using Godot;
using System;

public partial class Pause : Control
{
	public override void _Ready()
	{
		Hide();
	}

	// Method to handle "Continue" button press
	public void _on_Pause_Continue_pressed()
	{
		GetTree().Paused = false; 
		Hide();
	}

	// Method to handle "Exit" button press
	public void _on_Pause_Exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
