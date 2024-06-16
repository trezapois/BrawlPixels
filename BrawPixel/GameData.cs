using Godot;
using System;

public partial class GameData : Node
{
    public string SelectedCharacter { get; set; }
    public string SelectedStage { get; set; }

    public override void _Ready()
    {
        SelectedCharacter = "";
        SelectedStage = "";
    }
}