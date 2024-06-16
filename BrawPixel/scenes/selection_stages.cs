using Godot;
using System;

public partial class selection_stages : Control
{
	public override void _Ready()
	{
		var volcanoButton = GetNode<Button>("Volcano");

		// Check if the signal is already connected before connecting
		if (!volcanoButton.IsConnected("pressed", new Callable(this, nameof(OnVolcanoButtonPressed))))
		{
			volcanoButton.Connect("pressed", new Callable(this, nameof(OnVolcanoButtonPressed)));
		}
	}

	private void OnVolcanoButtonPressed()
	{
		StoreSelectedStage("Volcano");
		StartGame();
	}

	private void StoreSelectedStage(string stage)
	{
		var gameData = GetNode<GameData>("/root/GameData");
		gameData.SelectedStage = stage;
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
