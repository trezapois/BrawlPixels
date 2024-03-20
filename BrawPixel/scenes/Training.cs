using Godot;
using System;

public partial class Training : Node
{
	private Control pauseMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("Pause");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_pause_2_pressed(){
		GetTree().Paused= true;
		pauseMenu.Show();
	}
	public void _on_continue_pressed(){
		Console.WriteLine("1");
		pauseMenu.Hide();
		GetTree().Paused= false;
	}
	public void _on_exit_pressed()
    {
		GetTree().Paused= false;
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
}
