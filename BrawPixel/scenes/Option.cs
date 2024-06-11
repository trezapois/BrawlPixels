using Godot;
using System;

public partial class Option : Node
{
	private AudioStreamPlayer _audioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the AudioStreamPlayer node
		_audioPlayer = GetNode<AudioStreamPlayer>("ExitSound");

		// Connect the button's "pressed" signal to the OnVolumePress method
		var volumeButton = GetNode<Button>("Volume");
		volumeButton.Connect("pressed", new Callable(this, nameof(OnVolumePress)));

		// Connect the button's "pressed" signal to the OnExitPress method
		var exitButton = GetNode<Button>("Exit");
		exitButton.Connect("pressed", new Callable(this, nameof(OnExitPress)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnVolumePress()
	{
		GetTree().ChangeSceneToFile("");
	}

	public void OnExitPress()
	{
		GD.Print("Exit button pressed");

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
		timer.Connect("timeout", new Callable(this, nameof(OnExitSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnExitSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
