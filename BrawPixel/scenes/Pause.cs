using Godot;
using System;

public partial class Pause : Control
{


	public override void _Ready()
	{
		Hide();
	}


	public void _on_continue_pressed()
	{
		GetTree().Paused = false; 
		Hide();
	}

	// Function to handle "Exit" button press
	public void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
