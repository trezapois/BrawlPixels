using Godot;
using System;

public partial class Option : Node
{
	private AudioStreamPlayer _audioPlayer;
	private AudioStreamPlayer _volumeSoundPlayer;
	private OptionMusicManager _optionMusicManager;

	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("ExitSound");
		_volumeSoundPlayer = GetNode<AudioStreamPlayer>("VolumeSound");

		var volumeButton = GetNode<Button>("Volume");
		if (!volumeButton.IsConnected("pressed", new Callable(this, nameof(OnVolumePress))))
		{
			volumeButton.Connect("pressed", new Callable(this, nameof(OnVolumePress)));
		}

		var exitButton = GetNode<Button>("Exit");
		if (!exitButton.IsConnected("pressed", new Callable(this, nameof(OnExitPress))))
		{
			exitButton.Connect("pressed", new Callable(this, nameof(OnExitPress)));
		}

		// Instantiate and add the OptionMusicManager to the scene
		var optionMusicManagerScene = (PackedScene)ResourceLoader.Load("res://scenes/option_music_manager.tscn");
		_optionMusicManager = (OptionMusicManager)optionMusicManagerScene.Instantiate();
		AddChild(_optionMusicManager);

		// Start the option music
		_optionMusicManager.PlayMusic();
	}

	public void OnVolumePress()
	{
		GD.Print("Volume button pressed");

		var backgroundMusicManager = GetNode<BackgroundMusicManager>("/root/BackgroundMusicManager");
		backgroundMusicManager.StopMusic();

		if (_volumeSoundPlayer != null)
		{
			_volumeSoundPlayer.Stop();
			_volumeSoundPlayer.Play();
		}
		else
		{
			GD.Print("Volume Player not found: ");
		}

		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnVolumeSceneChange)));
		timer.Start(0.1f);
	}

	private void OnVolumeSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Volume.tscn");

		var backgroundMusicManager = GetNode<BackgroundMusicManager>("/root/BackgroundMusicManager");
		backgroundMusicManager.ResumeMusic();
	}

	public void OnExitPress()
	{
		GD.Print("Exit button pressed");

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
		timer.Connect("timeout", new Callable(this, nameof(OnExitSceneChange)));
		timer.Start(0.1f);
	}

	private void OnExitSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
