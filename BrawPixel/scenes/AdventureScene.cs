using Godot;
using System;

public partial class AdventureScene : Control
{
	private Button _stage1;
	private Button _stage2;
	private Button _stage3;
	private Sprite2D _chain2;
	private Sprite2D _chain3;
	private int _completedStages;
	private Button _backButton;  // Add this line to declare the Back button

	public override void _Ready()
	{
		// Get references to the stage buttons
		_stage1 = GetNode<Button>("Stage1");
		_stage2 = GetNode<Button>("Stage2");
		_stage3 = GetNode<Button>("Stage3");
		_backButton = GetNode<Button>("Back");  // Add this line to get the Back button node

		// Get references to the chain sprites
		_chain2 = GetNode<Sprite2D>("Stage2/Chain1");
		_chain3 = GetNode<Sprite2D>("Stage3/Chain1");

		// Initially disable Stage 2 and Stage 3
		_stage2.Disabled = true;
		_stage3.Disabled = true;

		// Connect signals to stage buttons
		_stage1.Pressed += OnStage1Pressed;
		_stage2.Pressed += OnStage2Pressed;
		_stage3.Pressed += OnStage3Pressed;
		_backButton.Pressed += OnBackPressed;  // Add this line to connect the Back button signal

		// Load completed stages from a saved file or another mechanism if you have it
		LoadProgress();
	}

	private void LoadProgress()
	{
		// For simplicity, we're assuming a variable that stores completed stages
		// This should ideally come from a save file or player prefs
		_completedStages = 0; // Change this to your saved progress

		// Update button states based on progress
		UpdateStageAvailability();
	}

	private void UpdateStageAvailability()
	{
		if (_completedStages >= 1)
		{
			_stage2.Disabled = false;
			_chain2.Visible = false;
		}
		else
		{
			_chain2.Visible = true;
		}

		if (_completedStages >= 2)
		{
			_stage3.Disabled = false;
			_chain3.Visible = false;
		}
		else
		{
			_chain3.Visible = true;
		}
	}

	private void OnStage1Pressed()
	{
		// Logic for Stage 1
		GD.Print("Stage 1 selected");

		// Mark stage 1 as completed and update progress
		_completedStages = Math.Max(_completedStages, 1);
		UpdateStageAvailability();

		// Change scene to Stage1.tscn or your actual stage scene
		GetTree().ChangeSceneToFile("res://scenes/Stage1.tscn");
	}

	private void OnStage2Pressed()
	{
		// Logic for Stage 2
		GD.Print("Stage 2 selected");

		// Mark stage 2 as completed and update progress
		_completedStages = Math.Max(_completedStages, 2);
		UpdateStageAvailability();

		// Change scene to Stage2.tscn or your actual stage scene
		GetTree().ChangeSceneToFile("res://scenes/Stage2.tscn");
	}

	private void OnStage3Pressed()
	{
		// Logic for Stage 3
		GD.Print("Stage 3 selected");

		// Mark stage 3 as completed
		_completedStages = Math.Max(_completedStages, 3);
		UpdateStageAvailability();

		// Change scene to Stage3.tscn or your actual stage scene
		GetTree().ChangeSceneToFile("res://scenes/Stage3.tscn");
	}

	private void OnBackPressed()
	{
		// Logic for Back button
		GD.Print("Back button pressed");

		// Change scene to Play.tscn or your actual main menu scene
		GetTree().ChangeSceneToFile("res://scenes/Play.tscn");
	}
}
