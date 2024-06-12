using Godot;
using System;

public partial class Play : Node
{
	private AudioStreamPlayer _exitAudioPlayer;
	private AudioStreamPlayer _trainingAudioPlayer;
	private AudioStreamPlayer _multiAudioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the AudioStreamPlayer nodes
		_exitAudioPlayer = GetNode<AudioStreamPlayer>("ExitSound");

		if (_exitAudioPlayer == null)
		{
			GD.Print("ExitSound node not found");
		}
		else
		{
			GD.Print("ExitSound node found");
		}

		_trainingAudioPlayer = GetNode<AudioStreamPlayer>("Train&MultiSound");

		if (_trainingAudioPlayer == null)
		{
			GD.Print("TrainingSound node not found");
		}
		else
		{
			GD.Print("TrainingSound node found");
		}

		_multiAudioPlayer = GetNode<AudioStreamPlayer>("Train&MultiSound");

		if (_multiAudioPlayer == null)
		{
			GD.Print("MultiSound node not found");
		}
		else
		{
			GD.Print("MultiSound node found");
		}

		// Connect the button's "pressed" signal to the OnExitPress method
		var exitButton = GetNode<Button>("Exit");
		if (!exitButton.IsConnected("pressed", new Callable(this, nameof(OnExitPress))))
		{
			exitButton.Connect("pressed", new Callable(this, nameof(OnExitPress)));
		}

		// Connect the button's "pressed" signal to the OnMultiPress method
		var multiButton = GetNode<Button>("Multiplayer");
		if (!multiButton.IsConnected("pressed", new Callable(this, nameof(OnMultiPress))))
		{
			multiButton.Connect("pressed", new Callable(this, nameof(OnMultiPress)));
		}

		// Connect the button's "pressed" signal to the OnTrainingPress method
		var trainingButton = GetNode<Button>("Training");
		if (!trainingButton.IsConnected("pressed", new Callable(this, nameof(OnTrainingPress))))
		{
			trainingButton.Connect("pressed", new Callable(this, nameof(OnTrainingPress)));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnExitPress()
	{
		GD.Print("Exit button pressed");

		// Play the sound effect
		if (_exitAudioPlayer != null)
		{
			_exitAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_exitAudioPlayer.Play(); // Then play the sound
		}
		else
		{
			GD.Print("Audio player not found");
		}

		// Change the scene after a short delay to ensure the sound plays
		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnExitSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnExitSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	public void OnMultiPress()
	{
		GD.Print("Multi button pressed");

		// Play the sound effect
		if (_multiAudioPlayer != null)
		{
			_multiAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_multiAudioPlayer.Play(); // Then play the sound
		}
		else
		{
			GD.Print("Multi sound player not found");
		}

		// Change the scene after a short delay to ensure the sound plays
		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnMultiSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnMultiSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/host_and_join.tscn");
	}

	public void OnTrainingPress()
	{
		GD.Print("Training button pressed");

		// Play the sound effect
		if (_trainingAudioPlayer != null)
		{
			_trainingAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_trainingAudioPlayer.Play(); // Then play the sound
		}
		else
		{
			GD.Print("Training sound player not found");
		}

		// Change the scene after a short delay to ensure the sound plays
		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnTrainingSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnTrainingSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Training.tscn");
	}
}
