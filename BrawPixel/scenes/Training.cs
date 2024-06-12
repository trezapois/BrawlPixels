using Godot;
using System;

public partial class Training : Node
{
	private AudioStreamPlayer _exitAudioPlayer;
	private AudioStreamPlayer _continueAudioPlayer;
	private AudioStreamPlayer _pauseAudioPlayer;
	private Control pauseMenu;
	private CharacterBody2D buddy;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("Pause");
		buddy = GetNode<CharacterBody2D>("Buddy");

		// Get the AudioStreamPlayer nodes
		_exitAudioPlayer = GetNode<AudioStreamPlayer>("ExitSound");
		_continueAudioPlayer = GetNode<AudioStreamPlayer>("ContinueSound");
		_pauseAudioPlayer = GetNode<AudioStreamPlayer>("PauseSound");

		GD.Print(_exitAudioPlayer != null ? "ExitSound node found" : "ExitSound node not found");
		GD.Print(_continueAudioPlayer != null ? "ContinueSound node found" : "ContinueSound node not found");
		GD.Print(_pauseAudioPlayer != null ? "PauseSound node found" : "PauseSound node not found");

		// Explicitly connect signals
		var exitButton = GetNode<Button>("Pause/Pause@Exit");
		exitButton.Connect("pressed", new Callable(this, nameof(OnExitPressed)));

		var continueButton = GetNode<Button>("Pause/Pause@Continue");
		continueButton.Connect("pressed", new Callable(this, nameof(OnContinuePressed)));

		var pauseButton = GetNode<Button>("Pause2");
		pauseButton.Connect("pressed", new Callable(this, nameof(_on_pause_2_pressed)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (buddy != null && !buddy.Visible)
		{
			OnBuddyDied();
		}
	}

	public void _on_pause_2_pressed()
	{
		GD.Print("Pause button pressed");

		// Play the sound effect
		if (_pauseAudioPlayer != null)
		{
			_pauseAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_pauseAudioPlayer.Play(); // Then play the sound
			GD.Print("Playing pause sound");
		}
		else
		{
			GD.Print("Pause sound player not found");
		}

		GetTree().Paused = true;
		pauseMenu.Show();
	}

	public void OnContinuePressed()
	{
		GD.Print("Continue button pressed");

		// Play the sound effect
		if (_continueAudioPlayer != null)
		{
			_continueAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_continueAudioPlayer.Play(); // Then play the sound
			GD.Print("Playing continue sound");
		}
		else
		{
			GD.Print("Continue sound player not found");
		}

		pauseMenu.Hide();
		GetTree().Paused = false;
	}

	public void OnExitPressed()
	{
		GD.Print("Exit button pressed");

		// Play the sound effect
		if (_exitAudioPlayer != null)
		{
			_exitAudioPlayer.Stop(); // Ensure the audio player is stopped first
			_exitAudioPlayer.Play(); // Then play the sound
			GD.Print("Playing exit sound");

			// Change the scene immediately after playing the sound
			OnExitSceneChange();
		}
		else
		{
			GD.Print("Exit sound player not found");
		}
	}

	private void OnExitSceneChange()
	{
		GD.Print("Changing to main menu scene");
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void OnBuddyDied()
	{
		// Transition to the "you won" scene
		GetTree().ChangeSceneToFile("res://scenes/Win.tscn");
	}
}
