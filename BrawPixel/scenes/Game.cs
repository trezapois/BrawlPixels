using Godot;
using System;

public partial class Game : Node2D
{
    // Variables to store the selected character and stage
    private string selectedCharacter;
    private string selectedStage;

    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        // Retrieve selected character and stage from global GameData
        var gameData = GetNode<GameData>("/root/GameData");
        selectedCharacter = gameData.SelectedCharacter;
        selectedStage = gameData.SelectedStage;

        // Print the selected character and stage for debugging
        GD.Print("Selected Character: " + selectedCharacter);
        GD.Print("Selected Stage: " + selectedStage);

        // Load and instantiate the selected stage
        LoadSelectedStage();
        // Load and instantiate the selected character
        LoadSelectedCharacter();
        // Load and instantiate the buddy character
        LoadBuddyCharacter();
    }

    // Method to load and position the selected character
    private void LoadSelectedCharacter()
    {
        PackedScene characterScene = null;

        // Determine which character scene to load based on the selected character
        if (selectedCharacter == "PurpleMan")
        {
            characterScene = (PackedScene)GD.Load("res://scenes/Main_character.tscn");
        }
        else if (selectedCharacter == "Hilda")
        {
            characterScene = (PackedScene)GD.Load("res://scenes/hilda.tscn");
        }
        else if (selectedCharacter == "RetroBoy")
        {
            characterScene = (PackedScene)GD.Load("res://scenes/retro_boy.tscn");
        }

        // If the character scene was successfully loaded, instantiate and add it to the scene
        if (characterScene != null)
        {
            var characterInstance = characterScene.Instantiate<Node2D>();
            AddChild(characterInstance);
            GD.Print("Character instance added");

            // Position the character at the spawn point within the stage
            var stageRoot = GetNode<Node2D>("StageRoot");
            if (stageRoot != null)
            {
                GD.Print("StageRoot found");
                var spawnPoint = stageRoot.GetNode<Node2D>("CharacterSpawn");
                if (spawnPoint != null)
                {
                    // Set the character's position to the spawn point's position
                    characterInstance.Position = stageRoot.Position + spawnPoint.Position;
                    GD.Print("Character positioned at: " + characterInstance.Position);
                }
                else
                {
                    GD.PrintErr("CharacterSpawn node not found in stage");
                }
            }
            else
            {
                GD.PrintErr("StageRoot node not found");
            }

            // Ensure the character is visible
            characterInstance.Visible = true;
        }
        else
        {
            GD.PrintErr("Failed to load character scene: " + selectedCharacter);
        }
    }

    // Method to load and position the buddy character
    private void LoadBuddyCharacter()
    {
        // Load the buddy character scene
        PackedScene buddyScene = (PackedScene)GD.Load("res://scenes/buddy.tscn");

        // If the buddy character scene was successfully loaded, instantiate and add it to the scene
        if (buddyScene != null)
        {
            var buddyInstance = buddyScene.Instantiate<Node2D>();
            AddChild(buddyInstance);
            GD.Print("Buddy character instance added");

            // Position the buddy at the buddy spawn point within the stage
            var stageRoot = GetNode<Node2D>("StageRoot");
            if (stageRoot != null)
            {
                var buddySpawnPoint = stageRoot.GetNode<Node2D>("BuddySpawn");
                if (buddySpawnPoint != null)
                {
                    // Set the buddy's position to the buddy spawn point's position
                    buddyInstance.Position = stageRoot.Position + buddySpawnPoint.Position;
                    GD.Print("Buddy positioned at: " + buddyInstance.Position);
                }
                else
                {
                    GD.PrintErr("BuddySpawn node not found in stage");
                }
            }
            else
            {
                GD.PrintErr("StageRoot node not found");
            }

            // Ensure the buddy is visible
            buddyInstance.Visible = true;
        }
        else
        {
            GD.PrintErr("Failed to load buddy scene.");
        }
    }

    // Method to load and instantiate the selected stage
    private void LoadSelectedStage()
    {
        PackedScene stageScene = null;

        // Determine which stage scene to load based on the selected stage
        if (selectedStage == "Volcano")
        {
            stageScene = (PackedScene)GD.Load("res://scenes/volcano.tscn");
        }

        // If the stage scene was successfully loaded, instantiate and add it to the scene
        if (stageScene != null)
        {
            var stageInstance = stageScene.Instantiate<Node2D>();
            stageInstance.Name = "StageRoot"; // Name the stage instance so we can reference it
            AddChild(stageInstance);
            stageInstance.Position = new Vector2(0, 0); // Adjust as necessary
            GD.Print("Stage loaded and instantiated at position: " + stageInstance.Position);
        }
        else
        {
            GD.PrintErr("Failed to load stage scene: " + selectedStage);
        }
    }
}
