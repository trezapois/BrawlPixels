using Godot;
using System;

public partial class Win : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
public void _on_exit_pressed(){
	GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
}
	
}
