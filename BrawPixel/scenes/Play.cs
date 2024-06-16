using Godot;
using System;

public partial class Play : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnExitPress(){
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
	public void OnMultiPress(){
		GetTree().ChangeSceneToFile("res://scenes/host_and_join.tscn");
	}
	public void OnTrainingPress(){
		GetTree().ChangeSceneToFile("res://scenes/selection_Character.tscn");
	}
}
