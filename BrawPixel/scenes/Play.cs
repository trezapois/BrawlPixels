using Godot;
using System;

public partial class Play : Node
{
	private AudioStreamPlayer _exitAudioPlayer;
	private AudioStreamPlayer _trainingAudioPlayer;
	private AudioStreamPlayer _multiAudioPlayer;
	private AudioStreamPlayer _currentMusicPlayer;
	private Random _random = new Random();

	public override void _Ready()
	{
		// Get the AudioStreamPlayer nodes
		_exitAudioPlayer = GetNode<AudioStreamPlayer>("ExitSound");
		_trainingAudioPlayer = GetNode<AudioStreamPlayer>("Train&MultiSound");
		_multiAudioPlayer = GetNode<AudioStreamPlayer>("Train&MultiSound");

		// Check if nodes are found
		GD.Print(_exitAudioPlayer != null ? "ExitSound node found" : "ExitSound node not found");
		GD.Print(_trainingAudioPlayer != null ? "TrainingSound node found" : "TrainingSound node not found");
		GD.Print(_multiAudioPlayer != null ? "MultiSound node found" : "MultiSound node not found");

		// Get the button nodes from the VBoxContainer
		var vboxContainer = GetNode<VBoxContainer>("VBoxContainer");
		var exitButton = vboxContainer.GetNode<Button>("Exit");
		var multiButton = vboxContainer.GetNode<Button>("Multiplayer");
		var trainingButton = vboxContainer.GetNode<Button>("Training");
		var storyButton = vboxContainer.GetNode<Button>("StoryMode");

		// Connect the buttons' "pressed" signals if not already connected
		if (!exitButton.IsConnected("pressed", new Callable(this, nameof(OnExitPress))))
		{
			exitButton.Connect("pressed", new Callable(this, nameof(OnExitPress)));
		}

		if (!multiButton.IsConnected("pressed", new Callable(this, nameof(OnMultiPress))))
		{
			multiButton.Connect("pressed", new Callable(this, nameof(OnMultiPress)));
		}

		if (!trainingButton.IsConnected("pressed", new Callable(this, nameof(OnTrainingPress))))
		{
			trainingButton.Connect("pressed", new Callable(this, nameof(OnTrainingPress)));
		}

		if (!storyButton.IsConnected("pressed", new Callable(this, nameof(OnStoryPress))))
		{
			storyButton.Connect("pressed", new Callable(this, nameof(OnStoryPress)));
		}

		// Start playing one of the background musics
		PlayRandomMusic();
	}

	private void PlayRandomMusic()
	{
		if (_currentMusicPlayer != null && _currentMusicPlayer.Playing)
		{
			_currentMusicPlayer.Stop();
		}

		_currentMusicPlayer = _random.Next(0, 2) == 0 ? GetNode<AudioStreamPlayer>("MusicChill1") : GetNode<AudioStreamPlayer>("MusicChill2");
		_currentMusicPlayer.Play();
	}

	public override void _Process(double delta)
	{
	}

	public void OnExitPress()
	{
		GD.Print("Exit button pressed");

		// Stop the background music
		if (_currentMusicPlayer != null && _currentMusicPlayer.Playing)
		{
			_currentMusicPlayer.Stop();
		}

		// Play the sound effect
		_exitAudioPlayer?.Play();

		// Change the scene after a short delay to ensure the sound plays
		var timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnExitSceneChange)));
		timer.Start(0.1f);
	}

	private void OnExitSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	public void OnMultiPress()
	{
		GD.Print("Multi button pressed");

		// Stop the background music
		_currentMusicPlayer?.Stop();

		// Play the sound effect
		_multiAudioPlayer?.Play();

		// Change the scene after a short delay to ensure the sound plays
		var timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnMultiSceneChange)));
		timer.Start(0.1f);
	}

	private void OnMultiSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/host_and_join.tscn");
	}

	public void OnTrainingPress()
	{
		GD.Print("Training button pressed");

		// Stop the background music
		_currentMusicPlayer?.Stop();

		// Play the sound effect
		_trainingAudioPlayer?.Play();

		// Change the scene after a short delay to ensure the sound plays
		var timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnTrainingSceneChange)));
		timer.Start(0.1f);
	}

	private void OnTrainingSceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Training.tscn");
	}

	public void OnStoryPress()
	{
		GD.Print("Story button pressed");

		// Stop the background music
		_currentMusicPlayer?.Stop();

		// Play the sound effect (if any, you can add a sound player for this)

		// Change the scene after a short delay to ensure the sound plays
		var timer = new Timer();
		AddChild(timer);
		timer.Connect("timeout", new Callable(this, nameof(OnStorySceneChange)));
		timer.Start(0.1f);
	}

	private void OnStorySceneChange()
	{
		GetTree().ChangeSceneToFile("res://scenes/Story.tscn"); // Make sure this path is correct
	}
}
