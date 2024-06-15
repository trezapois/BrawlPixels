using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] private int maxHits = 10;
	private int hitCount = 0;
	private Random random = new Random();

	[Signal]
	public delegate void EnemyDefeatedEventHandler();

	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));

		var movementTimer = GetNode<Timer>("Timer");
		movementTimer.Connect("timeout", new Callable(this, nameof(OnMovementTimeout)));
		movementTimer.Start();
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D player)
		{
			GD.Print("Enemy hit by player");
			hitCount++;
			if (hitCount >= maxHits)
			{
				EmitSignal(nameof(EnemyDefeatedEventHandler));
				QueueFree(); // Remove enemy when defeated
			}
			else
			{
				ChangePosition();
			}
		}
	}

	private void ChangePosition()
	{
		Position = new Vector2(random.Next(0, 1024), random.Next(0, 768)); // Adjust based on your stage size
	}

	private void OnMovementTimeout()
	{
		ChangePosition();
	}
}
