using Godot;
using System;

public partial class main_menu : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnPlayPress()
	{
		GD.Print("hit");
		GetTree().ChangeSceneToFile("res://scenes/Play.tscn");
	}
	public void OnOptionPress()
	{
		GetTree().ChangeSceneToFile("res://scenes/Option.tscn");
	}
}
