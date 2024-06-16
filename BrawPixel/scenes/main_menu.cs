using Godot;
using System;

public partial class main_menu : Node
{
	private AudioStreamPlayer _audioPlayer;

	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("MenuSound");

		var playButton = GetNode<Button>("Play");
		if (!playButton.IsConnected("pressed", new Callable(this, nameof(OnPlayPress))))
		{
			playButton.Connect("pressed", new Callable(this, nameof(OnPlayPress)));
		}

		var optionButton = GetNode<Button>("Option");
		if (!optionButton.IsConnected("pressed", new Callable(this, nameof(OnOptionPress))))
		{
			optionButton.Connect("pressed", new Callable(this, nameof(OnOptionPress)));
		}

		// Start the background music
		var backgroundMusicManager = GetNode<BackgroundMusicManager>("/root/BackgroundMusicManager");
		backgroundMusicManager.PlayMusic();
	}

	public void OnPlayPress()
	{
		GD.Print("Play button pressed");

		var backgroundMusicManager = GetNode<BackgroundMusicManager>("/root/BackgroundMusicManager");
		backgroundMusicManager.StopMusic();

		if (_audioPlayer != null)
		{
			_audioPlayer.Stop();
			_audioPlayer.Play();
		}
		else
		{
			GD.Print("Audio player not found");
		}

		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnPlaySceneChange)));
		timer.Start(0.1f);
	}

	private void OnPlaySceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Play.tscn");
	}

	public void OnOptionPress()
	{
		GD.Print("Option button pressed");

		var backgroundMusicManager = GetNode<BackgroundMusicManager>("/root/BackgroundMusicManager");
		backgroundMusicManager.StopMusic();

		if (_audioPlayer != null)
		{
			_audioPlayer.Stop();
			_audioPlayer.Play();
		}
		else
		{
			GD.Print("Audio player not found");
		}

		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnOptionSceneChange)));
		timer.Start(0.1f);
	}

	private void OnOptionSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Option.tscn");
	}
}
