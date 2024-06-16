using Godot;
using System;

public partial class selection_character3 : Node2D
{
	public override void _Ready()
	{
	}

	public void _on_purple_man_pressed()
	{
		StoreSelectedCharacter("Hilda");
		GetTree().ChangeSceneToFile("res://scenes/selection_stages.tscn");
	}

	private void StoreSelectedCharacter(string character)
	{
		var gameData = GetNode<GameData>("/root/GameData");
		gameData.SelectedCharacter = character;
	}
	private void _on_button_pressed()
{
	GetTree().ChangeSceneToFile("res://scenes/selection_Character2.tscn");
	// Replace with function body.
}
private void _on_button_2_pressed()
{
	GetTree().ChangeSceneToFile("res://scenes/selection_Character.tscn");
	// Replace with function body.
}
	public override void _Process(double delta)
	{
	}
}







