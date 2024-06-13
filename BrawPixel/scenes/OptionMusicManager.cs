using Godot;
using System;

public partial class OptionMusicManager : Node
{
	private AudioStreamPlayer _optionMusicPlayer;

	public override void _Ready()
	{
		_optionMusicPlayer = GetNode<AudioStreamPlayer>("Nostalgia");
		if (!_optionMusicPlayer.Playing)
		{
			_optionMusicPlayer.Play();
		}
	}

	public void StopMusic()
	{
		_optionMusicPlayer.Stop();
	}

	public void PlayMusic()
	{
		if (!_optionMusicPlayer.Playing)
		{
			_optionMusicPlayer.Play();
		}
	}

	public void PauseMusic()
	{
		_optionMusicPlayer.StreamPaused = true;
	}

	public void ResumeMusic()
	{
		_optionMusicPlayer.StreamPaused = false;
	}
}
