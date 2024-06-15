using Godot;
using System;

public partial class Stage2 : Node
{
	public AdventureScene AdventureScene { get; set; }

	// Similar implementation as Stage1 with stage-specific logic

	private AudioStreamPlayer _exitAudioPlayer;
	private AudioStreamPlayer _continueAudioPlayer;
	private AudioStreamPlayer _pauseAudioPlayer;
	private Control _pauseMenu;
	private Label _instructionLabel;
	private CharacterBody2D _player;
	private Area2D _groundDetector;
	private Control _completionPanel;
	private Button _completionExitButton;
	private Button _pauseButton;
	private int _currentStep = 0;
	private int _jumpCount = 0;
	private bool _isJumping = false;

	private string[] _instructions = new string[]
	{
		"Move right until you reach the edge.",
		"Now, move left until you reach the edge.",
		"Jump one time.",
		"Jump three times."
	};

	public override void _Ready()
	{
		_pauseMenu = GetNode<Control>("Pause");
		_exitAudioPlayer = GetNode<AudioStreamPlayer>("ExitSound");
		_continueAudioPlayer = GetNode<AudioStreamPlayer>("ContinueSound");
		_pauseAudioPlayer = GetNode<AudioStreamPlayer>("PauseSound");
		var exitButton = GetNode<Button>("Pause/Pause_Exit");
		exitButton.Connect("pressed", new Callable(this, nameof(OnExitPressed)));
		var continueButton = GetNode<Button>("Pause/Pause_Continue");
		continueButton.Connect("pressed", new Callable(this, nameof(OnContinuePressed)));
		_pauseButton = GetNode<Button>("Pause2");
		_pauseButton.Connect("pressed", new Callable(this, nameof(OnPause2Pressed)));
		_completionExitButton = GetNode<Button>("CompletionPanel/ExitButton");
		_completionExitButton.Connect("pressed", new Callable(this, nameof(OnCompletionExitPressed)));
		_instructionLabel = GetNode<Label>("DoIt");
		_player = GetNode<CharacterBody2D>("1");
		_instructionLabel.Text = _instructions[_currentStep];
		_pauseMenu.Hide();
		_completionPanel = GetNode<Control>("CompletionPanel");
		_completionPanel.Hide();
		GetNode<Area2D>("RightEnd").Connect("body_entered", new Callable(this, nameof(OnRightEndBodyEntered)));
		GetNode<Area2D>("LeftEnd").Connect("body_entered", new Callable(this, nameof(OnLeftEndBodyEntered)));
		_groundDetector = GetNode<Area2D>("GroundDetector");
		_groundDetector.Connect("body_entered", new Callable(this, nameof(OnGroundDetectorBodyEntered)));
		GetNode<TileMap>("TileMap").AddToGroup("ground");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_jump"))
		{
			_isJumping = true;
		}
	}

	private void OnPause2Pressed()
	{
		if (_pauseAudioPlayer != null)
		{
			_pauseAudioPlayer.Stop();
			_pauseAudioPlayer.Play();
		}

		GetTree().Paused = true;
		_pauseMenu.Show();
	}

	private void OnContinuePressed()
	{
		if (_continueAudioPlayer != null)
		{
			_continueAudioPlayer.Stop();
			_continueAudioPlayer.Play();
		}

		_pauseMenu.Hide();
		GetTree().Paused = false;
	}

	private void OnExitPressed()
	{
		if (_exitAudioPlayer != null)
		{
			_exitAudioPlayer.Stop();
			_exitAudioPlayer.Play();
			OnExitSceneChange();
		}
	}

	private void OnCompletionExitPressed()
	{
		if (_exitAudioPlayer != null)
		{
			_exitAudioPlayer.Stop();
			_exitAudioPlayer.Play();
			NotifyStageCompleted(2); // Directly notify parent
			OnExitSceneChange();
		}
	}

	private void OnExitSceneChange()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
	}

	private void OnRightEndBodyEntered(Node body)
	{
		if (body == _player && _currentStep == 0)
		{
			_currentStep++;
			_instructionLabel.Text = _instructions[_currentStep];
		}
	}

	private void OnLeftEndBodyEntered(Node body)
	{
		if (body == _player && _currentStep == 1)
		{
			_currentStep++;
			_instructionLabel.Text = _instructions[_currentStep];
		}
	}

	private void OnGroundDetectorBodyEntered(Node body)
	{
		if (_isJumping)
		{
			_isJumping = false;

			if (_currentStep == 2)
			{
				_jumpCount++;
				if (_jumpCount >= 1)
				{
					_currentStep++;
					_jumpCount = 0;
					_instructionLabel.Text = _instructions[_currentStep];
				}
			}
			else if (_currentStep == 3)
			{
				_jumpCount++;
				if (_jumpCount >= 3)
				{
					ShowCompletionPanel();
				}
			}
		}
	}

	private void ShowCompletionPanel()
	{
		_completionPanel.Show();
		_pauseMenu.Hide();
		_instructionLabel.Hide();
		_pauseButton.Hide();
	}

	private void NotifyStageCompleted(int stageNumber)
	{
		GD.Print("Stage 2 completed");
		AdventureScene?.OnStageCompleted(stageNumber);
	}
}
