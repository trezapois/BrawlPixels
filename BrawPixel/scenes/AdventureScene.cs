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
	private Node _stage1Instance;
	private Node _stage2Instance;
	private bool _blockExit = false;

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
		_backButton.Connect("gui_input", new Callable(this, nameof(OnBackButtonGuiInput)));

		UpdateStageAvailability();
	}

	public void BlockExit()
	{
		_blockExit = true;
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
		// Only act on button press if space is not pressed
		if (Input.IsActionPressed("ui_jump"))
		{
			GD.Print("Space button is pressed, not selecting Stage 1");
			return;  // Exit the method if space button is pressed
		}

		// If there's an existing instance, ensure it's freed
		if (_stage1Instance != null && IsInstanceValid(_stage1Instance))
		{
			_stage1Instance.QueueFree();
		}

		GD.Print("Stage 1 selected");
		var stage1Scene = (PackedScene)ResourceLoader.Load("res://scenes/Stage1.tscn");
		_stage1Instance = stage1Scene.Instantiate();
		((Stage1)_stage1Instance).AdventureScene = this;  // Set reference to parent scene
		GetTree().Root.AddChild(_stage1Instance);

		DisableButtonFocus();
	}

	private void OnStage2Pressed()
	{
		// Only act on button press if space is not pressed
		if (Input.IsActionPressed("ui_jump"))
		{
			GD.Print("Space button is pressed, not selecting Stage 2");
			return;  // Exit the method if space button is pressed
		}

		// If there's an existing instance, ensure it's freed
		if (_stage2Instance != null && IsInstanceValid(_stage2Instance))
		{
			_stage2Instance.QueueFree();
		}

		GD.Print("Stage 2 selected");
		var stage2Scene = (PackedScene)ResourceLoader.Load("res://scenes/Stage2.tscn");
		_stage2Instance = stage2Scene.Instantiate();
		((Stage2)_stage2Instance).AdventureScene = this;  // Set reference to parent scene
		GetTree().Root.AddChild(_stage2Instance);

		DisableButtonFocus();
	}

	private void OnStage3Pressed()
	{
		GD.Print("Stage 3 selected");
		GetTree().ChangeSceneToFile("res://scenes/Stage3.tscn");

		DisableButtonFocus();
	}

	private void OnBackButtonGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GD.Print("Back button pressed by mouse");
			if (_stage1Instance != null && IsInstanceValid(_stage1Instance))
			{
				_stage1Instance.QueueFree();
				_stage1Instance = null;
			}
			if (_stage2Instance != null && IsInstanceValid(_stage2Instance))
			{
				_stage2Instance.QueueFree();
				_stage2Instance = null;
			}
			_blockExit = false;  // Ensure exit is not blocked when back button is pressed
			GetTree().ChangeSceneToFile("res://scenes/Play.tscn");

			EnableButtonFocus();
		}
	}

	public void OnStageCompleted(int stageNumber)
	{
		var global = (Global)GetNode("/root/Global");
		if (stageNumber == 1)
		{
			global.SetStageCompleted(1, true);
			if (_stage1Instance != null && IsInstanceValid(_stage1Instance))
			{
				_stage1Instance.QueueFree();
				_stage1Instance = null;
			}
		}
		else if (stageNumber == 2)
		{
			global.SetStageCompleted(2, true);
			if (_stage2Instance != null && IsInstanceValid(_stage2Instance))
			{
				_stage2Instance.QueueFree();
				_stage2Instance = null;
			}
		}
		UpdateStageAvailability();

		EnableButtonFocus();
	}

	private void DisableButtonFocus()
	{
		_stage1.FocusMode = Control.FocusModeEnum.None;
		_stage2.FocusMode = Control.FocusModeEnum.None;
		_stage3.FocusMode = Control.FocusModeEnum.None;
		_backButton.FocusMode = Control.FocusModeEnum.None;
	}

	private void EnableButtonFocus()
	{
		_stage1.FocusMode = Control.FocusModeEnum.All;
		_stage2.FocusMode = Control.FocusModeEnum.All;
		_stage3.FocusMode = Control.FocusModeEnum.All;
		_backButton.FocusMode = Control.FocusModeEnum.All;
	}

	public override void _Input(InputEvent @event)
	{
		// Do nothing to prevent AdventureScene from handling inputs
	}
}
