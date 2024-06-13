using Godot;
using System;

public partial class Volume : Control
{
	private VolumeSlider masterSlider;
	private VolumeSlider musicSlider;
	private VolumeSlider sfxSlider;
	private Button exitButton;
	private AudioStreamPlayer exitSoundPlayer;
	private OptionMusicManager _optionMusicManager;

	public override void _Ready()
	{
		masterSlider = GetNode<VolumeSlider>("Volume_contents/master_slider");
		musicSlider = GetNode<VolumeSlider>("Volume_contents/music_slider");
		sfxSlider = GetNode<VolumeSlider>("Volume_contents/sfx_slider");
		exitButton = GetNode<Button>("Exit");
		exitSoundPlayer = GetNode<AudioStreamPlayer>("ExitSound");

		masterSlider.bus_name = "Master";
		musicSlider.bus_name = "Music";
		sfxSlider.bus_name = "SFX";

		exitButton.Connect("pressed", new Callable(this, nameof(OnExitButtonPressed)));

		// Initialize the slider values
		masterSlider.Value = DbToLinear(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master")));
		musicSlider.Value = DbToLinear(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Music")));
		sfxSlider.Value = DbToLinear(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("SFX")));

		// Instantiate and add the OptionMusicManager to the scene
		var optionMusicManagerScene = (PackedScene)ResourceLoader.Load("res://scenes/option_music_manager.tscn");
		_optionMusicManager = (OptionMusicManager)optionMusicManagerScene.Instantiate();
		AddChild(_optionMusicManager);

		// Start the option music
		_optionMusicManager.PlayMusic();
	}

	private void OnExitButtonPressed()
	{
		GD.Print("Exit button pressed");

		if (exitSoundPlayer != null)
		{
			exitSoundPlayer.Stop();
			exitSoundPlayer.Play();
		}
		else
		{
			GD.Print("ExitSound player not found");
		}

		Timer timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnExitSceneChange)));
		timer.Start(0.1f); // Start the timer with a short delay
	}

	private void OnExitSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Option.tscn");
	}

	private float DbToLinear(float db)
	{
		return Mathf.Pow(10.0f, db / 20.0f);
	}
}
