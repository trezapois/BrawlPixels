using Godot;
using System;

public partial class Stage2 : Node
{
	public AdventureScene AdventureScene { get; set; }

	private AudioStreamPlayer _exitAudioPlayer;
	private AudioStreamPlayer _backAudioPlayer;
	private AudioStreamPlayer _continueAudioPlayer;
	private AudioStreamPlayer _pauseAudioPlayer;
	private Control _pauseMenu;
	private Label _instructionLabel;
	private CharacterBody2D _player;
	private Control _completionPanel;
	private Button _completionExitButton;
	private Button _pauseButton;
	private int _currentStep = 0;
	private int _hitCount = 0;
	private bool _stageCompleted = false;
	private bool _isExiting = false;

	private string[] _instructions = new string[]
	{
		"Find and hit the enemy."
	};

	public override void _Ready()
	{
		try
		{
			GD.Print("Initializing nodes...");
			_pauseMenu = GetNodeOrNull<Control>("Pause");
			_exitAudioPlayer = GetNodeOrNull<AudioStreamPlayer>("ExitSound");
			_backAudioPlayer = GetNodeOrNull<AudioStreamPlayer>("BackSound");
			_continueAudioPlayer = GetNodeOrNull<AudioStreamPlayer>("ContinueSound");
			_pauseAudioPlayer = GetNodeOrNull<AudioStreamPlayer>("PauseSound");
			_instructionLabel = GetNodeOrNull<Label>("DoIt");
			_player = GetNodeOrNull<CharacterBody2D>("1");
			_completionPanel = GetNodeOrNull<Control>("CompletionPanel");
			_pauseButton = GetNodeOrNull<Button>("Pause2");
			_completionExitButton = GetNodeOrNull<Button>("CompletionPanel/ExitButton");

			if (_pauseMenu == null) GD.PrintErr("Pause menu is null.");
			if (_exitAudioPlayer == null) GD.PrintErr("Exit audio player is null.");
			if (_backAudioPlayer == null) GD.PrintErr("Back audio player is null.");
			if (_continueAudioPlayer == null) GD.PrintErr("Continue audio player is null.");
			if (_pauseAudioPlayer == null) GD.PrintErr("Pause audio player is null.");
			if (_instructionLabel == null) GD.PrintErr("Instruction label is null.");
			if (_player == null) GD.PrintErr("Player is null.");
			if (_completionPanel == null) GD.PrintErr("Completion panel is null.");
			if (_pauseButton == null) GD.PrintErr("Pause button is null.");
			if (_completionExitButton == null) GD.PrintErr("Completion exit button is null.");

			if (_pauseMenu == null || _exitAudioPlayer == null || _backAudioPlayer == null || _continueAudioPlayer == null || _pauseAudioPlayer == null || _instructionLabel == null || _player == null || _completionPanel == null || _pauseButton == null || _completionExitButton == null)
			{
				GD.PrintErr("One or more nodes are not found in the scene.");
				return;
			}

			var exitButton = GetNodeOrNull<Button>("Pause/Pause_Exit");
			if (exitButton != null)
				exitButton.Connect("pressed", new Callable(this, nameof(OnPauseExitPressed)));
			else
				GD.PrintErr("Pause exit button is null.");

			var continueButton = GetNodeOrNull<Button>("Pause/Pause_Continue");
			if (continueButton != null)
				continueButton.Connect("pressed", new Callable(this, nameof(OnContinuePressed)));
			else
				GD.PrintErr("Pause continue button is null.");

			_pauseButton.Connect("pressed", new Callable(this, nameof(OnPause2Pressed)));
			_completionExitButton.Connect("pressed", new Callable(this, nameof(OnCompletionExitPressed)));

			_instructionLabel.Text = _instructions[_currentStep];
			_pauseMenu.Hide();
			_completionPanel.Hide();

			GD.Print("Stage2 _Ready complete.");

			// Load and instantiate the enemy
			var enemyScene = (PackedScene)ResourceLoader.Load("res://Adventure/Enemy.tscn");
			var enemyInstance = (Enemy)enemyScene.Instantiate();
			enemyInstance.Connect("EnemyDefeated", new Callable(this, nameof(OnEnemyDefeated)));
			AddChild(enemyInstance);
		}
		catch (Exception ex)
		{
			GD.PrintErr("Error in _Ready: ", ex.ToString());
		}
	}

	private void OnPause2Pressed()
	{
		_pauseAudioPlayer?.Play();
		GetTree().Paused = true;
		_pauseMenu.Show();
	}

	private void OnContinuePressed()
	{
		_continueAudioPlayer?.Play();
		_pauseMenu.Hide();
		GetTree().Paused = false;
	}

	private void OnPauseExitPressed()
	{
		GD.Print("Pause Exit button pressed");
		_exitAudioPlayer.Play();
		if (_exitAudioPlayer != null)
		{
			GD.Print("Playing exit sound");
			_exitAudioPlayer.Seek(0);
			_exitAudioPlayer.Stop();
			_exitAudioPlayer.Play();
			CleanupStage2();
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
		}
		else
		{
			GD.PrintErr("Exit audio player is null!");
			OnPauseExitTimeout(); // Fallback if no audio player
		}
	}

	private void OnPauseExitTimeout()
	{
		if (_isExiting)
		{
			GD.Print("Exit already in progress, ignoring additional requests.");
			return;
		}
		_isExiting = true;

		GD.Print("Exit sound finished, changing scene");
		if (_exitAudioPlayer.IsConnected("finished", new Callable(this, nameof(OnPauseExitTimeout))))
		{
			_exitAudioPlayer.Disconnect("finished", new Callable(this, nameof(OnPauseExitTimeout)));
		}
		CleanupStage2();
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
	}

	private void CleanupStage2()
	{
		QueueFree(); // Free the current instance
	}

	private void OnCompletionExitPressed()
	{
		GD.Print("Completion Exit button pressed");
		_backAudioPlayer?.Play();
		NotifyStageCompleted(2);
		OnPauseExitTimeout(); // Directly call scene change
	}

	private void OnEnemyDefeated()
	{
		_hitCount++;
		GD.Print("Enemy defeated. Hit count: " + _hitCount);

		if (_hitCount >= 10)
		{
			ShowCompletionPanel();
		}
	}

	private void ShowCompletionPanel()
	{
		_completionPanel.Show();
		_pauseMenu.Hide();
		_instructionLabel.Hide();
		_pauseButton.Hide();
		_stageCompleted = true;
	}

	private void NotifyStageCompleted(int stageNumber)
	{
		GD.Print("Stage 2 completed");
		AdventureScene?.OnStageCompleted(stageNumber);
	}

	public override void _ExitTree()
	{
		if (!_stageCompleted && IsInstanceValid(AdventureScene))
		{
			AdventureScene.OnStageCompleted(-1); // Indicate that the stage was exited without completion
		}
	}
}
