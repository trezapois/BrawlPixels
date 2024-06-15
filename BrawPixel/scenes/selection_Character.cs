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

	public void _on_hilda_pressed()
	{
		StoreSelectedCharacter("Hilda");
		GetTree().ChangeSceneToFile("res://scenes/selection_stages.tscn");
	}

	public void _on_retro_boy_pressed()
	{
		StoreSelectedCharacter("RetroBoy");
		GetTree().ChangeSceneToFile("res://scenes/selection_stages.tscn");
	}

	private void StoreSelectedCharacter(string character)
	{
		var gameData = GetNode<GameData>("/root/GameData");
		gameData.SelectedCharacter = character;
	}

	public override void _Process(double delta)
	{
	}
}
