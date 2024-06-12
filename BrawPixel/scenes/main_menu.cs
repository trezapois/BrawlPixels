using Godot;
using System;

public partial class main_menu : Node
{
	private AudioStreamPlayer _audioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the AudioStreamPlayer node
		_audioPlayer = GetNode<AudioStreamPlayer>("MenuSound");

		// Get the Play button and connect the signal
		var playButton = GetNode<Button>("Play");
		if (!playButton.IsConnected("pressed", new Callable(this, nameof(OnPlayPress))))
		{
			playButton.Connect("pressed", new Callable(this, nameof(OnPlayPress)));
		}

		// Get the Option button and connect the signal
		var optionButton = GetNode<Button>("Option");
		if (!optionButton.IsConnected("pressed", new Callable(this, nameof(OnOptionPress))))
		{
			optionButton.Connect("pressed", new Callable(this, nameof(OnOptionPress)));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayPress()
	{
		GD.Print("Play button pressed");

		// Play the sound effect
		if (_audioPlayer != null)
		{
			_audioPlayer.Stop(); // Ensure the audio player is stopped first
			_audioPlayer.Play(); // Then play the sound
		}
		else
		{
			GD.Print("Audio player not found");
		}

		// Change the scene after a short delay to ensure the sound plays
		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnPlaySceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnPlaySceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Play.tscn");
	}

	public void OnOptionPress()
	{
		GD.Print("Option button pressed");

		// Play the sound effect
		if (_audioPlayer != null)
		{
			_audioPlayer.Stop(); // Ensure the audio player is stopped first
			_audioPlayer.Play(); // Then play the sound
		}
		else
		{
			GD.Print("Audio player not found");
		}

		// Change the scene after a short delay to ensure the sound plays
		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnOptionSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnOptionSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Option.tscn");
	}
}
