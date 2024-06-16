using Godot;
using System;
using System.Collections.Generic;
using Test.scenes;

public partial class Multi : Node2D
{
	[Export]
	private PackedScene playerScene;
	private Control pauseMenu;

public override void _Ready()
{
	pauseMenu = GetNode<Control>("Pause");
	int index = 0;
	foreach (var playerInfo in GameManager.Players)
	{
		
		Purple_Man currentPlayer = playerScene.Instantiate<Purple_Man>(); // take the purpleman character
		currentPlayer.Name = playerInfo.Id.ToString(); 
		AddChild(currentPlayer); // adding player
		
		GD.Print("Spawning player ", currentPlayer.Name, " at index ", index); // part to test and debug but works fine now so ignore this line
		foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoints"))
		{
			GD.Print("Spawn point name: ", spawnPoint.Name); // ignore this was just use to debug chracter not spawning
			if (int.Parse(spawnPoint.Name) == index)
			{
				// Assign player's position relative to spawn point
				currentPlayer.Position = spawnPoint.Position;
				GD.Print("Assigned spawn point ", spawnPoint.Name, " to player ", currentPlayer.Name);
				GD.Print("Player global position: ", currentPlayer.GlobalPosition);
				break;
			}
		}

		index++;
	}
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
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
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
	public override void _Process(double delta)
	{
		
	}
	
}
