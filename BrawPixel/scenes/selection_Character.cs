using Godot;
using System;

public partial class selection_Character : Control
{
	public override void _Ready()
	{
	}

	public void _on_purple_man_pressed()
	{
		GD.Print("1"); // Debug test
		StoreSelectedCharacter("PurpleMan");
		GetTree().ChangeSceneToFile("res://scenes/selection_stages.tscn");
	}

	private void StoreSelectedCharacter(string character)
	{
		var gameData = GetNode<GameData>("/root/GameData");
		gameData.SelectedCharacter = character;
	}
	private void _on_button_pressed()
{
	GetTree().ChangeSceneToFile("res://scenes/selection_character3.tscn");
	// Replace with function body.
}
private void _on_button_2_pressed()
{
	GetTree().ChangeSceneToFile("res://scenes/selection_Character2.tscn");
	// Replace with function body.
}
	public override void _Process(double delta)
	{
	}
}






