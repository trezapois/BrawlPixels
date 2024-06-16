using Godot;
using System;

public partial class volcano : Node2D
{
	private Control pauseMenu;
	private CharacterBody2D buddy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("Pause");
		buddy = GetNode<CharacterBody2D>("Buddy");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (buddy != null && !buddy.Visible)
		{
			OnBuddyDied();
		}
	}
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
		GetTree().Paused= false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
 	private void OnBuddyDied()
	{
		// Transition to the "you won" scene
		GetTree().ChangeSceneToFile("res://scenes/Win.tscn");
	}
}
