using Godot;
using System;

public partial class AdventureScene : Control
{
	private Button _stage1;
	private Button _stage2;
	private Button _stage3;
	private Sprite2D _chain2;
	private Sprite2D _chain3;
	private Button _backButton;
	private bool _stage1InstanceExists = false;
	private Node _stage1Instance;

	public override void _Ready()
	{
		_stage1 = GetNode<Button>("Stage1");
		_stage2 = GetNode<Button>("Stage2");
		_stage3 = GetNode<Button>("Stage3");
		_backButton = GetNode<Button>("Back");

		_chain2 = GetNode<Sprite2D>("Stage2/Chain1");
		_chain3 = GetNode<Sprite2D>("Stage3/Chain1");

		_stage1.Pressed += OnStage1Pressed;
		_stage2.Pressed += OnStage2Pressed;
		_stage3.Pressed += OnStage3Pressed;
		_backButton.Pressed += OnBackPressed;

		UpdateStageAvailability();
	}

	private void UpdateStageAvailability()
	{
		var global = (Global)GetNode("/root/Global");
		
		_stage2.Disabled = !global.IsStageCompleted(1);
		_chain2.Visible = !global.IsStageCompleted(1);

		_stage3.Disabled = !global.IsStageCompleted(2);
		_chain3.Visible = !global.IsStageCompleted(2);
	}

	private void OnStage1Pressed()
	{
		if (_stage1InstanceExists)
		{
			GD.Print("Stage 1 instance already exists");
			return;
		}

		GD.Print("Stage 1 selected");
		var stage1Scene = (PackedScene)ResourceLoader.Load("res://scenes/Stage1.tscn");
		_stage1Instance = stage1Scene.Instantiate();
		((Stage1)_stage1Instance).AdventureScene = this;  // Set reference to parent scene
		GetTree().Root.AddChild(_stage1Instance);

		_stage1InstanceExists = true;
	}

	private void OnStage2Pressed()
	{
		GD.Print("Stage 2 selected");
		var stage2Scene = (PackedScene)ResourceLoader.Load("res://scenes/Stage2.tscn");
		var stage2Instance = (Stage2)stage2Scene.Instantiate();
		stage2Instance.AdventureScene = this;  // Set reference to parent scene
		GetTree().Root.AddChild(stage2Instance);
	}

	private void OnStage3Pressed()
	{
		GD.Print("Stage 3 selected");
		GetTree().ChangeSceneToFile("res://scenes/Stage3.tscn");
	}

	private void OnBackPressed()
	{
		GD.Print("Back button pressed");
		if (_stage1InstanceExists && IsInstanceValid(_stage1Instance))
		{
			_stage1Instance.QueueFree();
			_stage1InstanceExists = false;
		}
		GetTree().ChangeSceneToFile("res://scenes/Play.tscn");
	}

	public void OnStageCompleted(int stageNumber)
	{
		var global = (Global)GetNode("/root/Global");

		if (stageNumber == 1)
		{
			global.SetStageCompleted(1, true);
			_stage1InstanceExists = false;
			if (IsInstanceValid(_stage1Instance))
			{
				_stage1Instance.QueueFree();
			}
		}
		else if (stageNumber == 2)
		{
			global.SetStageCompleted(2, true);
		}
		UpdateStageAvailability();
	}
}
