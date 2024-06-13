using Godot;
using System;

public partial class BackgroundMusicManager : Node
{
	private AudioStreamPlayer _backgroundMusicPlayer;

	public override void _Ready()
	{
		_backgroundMusicPlayer = GetNode<AudioStreamPlayer>("BrawlPixelTheme");
		if (!_backgroundMusicPlayer.Playing)
		{
			_backgroundMusicPlayer.Play();
		}
	}

	public void StopMusic()
	{
		_backgroundMusicPlayer.Stop();
	}

	public void PlayMusic()
	{
		if (!_backgroundMusicPlayer.Playing)
		{
			_backgroundMusicPlayer.Play();
		}
	}

	public void PauseMusic()
	{
		_backgroundMusicPlayer.StreamPaused = true;
	}

	public void ResumeMusic()
	{
		_backgroundMusicPlayer.StreamPaused = false;
	}
}
