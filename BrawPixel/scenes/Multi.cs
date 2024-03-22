using Godot;
using System;

public partial class Multi : Node2D
{
	[Export]
	private PackedScene playerScene;
public override void _Ready()
{
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
	public override void _Process(double delta)
	{
	}
}
