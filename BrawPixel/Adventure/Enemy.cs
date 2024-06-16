using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] private int maxHits = 10;
	private int hitCount = 0;

	[Signal]
	public delegate void EnemyDefeatedEventHandler();

	private Sprite2D _sprite;
	private CollisionShape2D _collisionShape;
	private Rect2 _boundaryRect;
	private bool canBeHit = true;
	private Timer _hitCooldownTimer;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");

		var area = GetNode<Area2D>("Area2D");
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));

		var movementTimer = GetNode<Timer>("Timer");
		movementTimer.Connect("timeout", new Callable(this, nameof(OnMovementTimeout)));
		movementTimer.Start();

		_hitCooldownTimer = new Timer();
		_hitCooldownTimer.WaitTime = 0.5f; // 0.5 second cooldown between hits
		_hitCooldownTimer.OneShot = true;
		_hitCooldownTimer.Connect("timeout", new Callable(this, nameof(OnHitCooldownTimeout)));
		AddChild(_hitCooldownTimer);

		_sprite.Scale = new Vector2(0.1f, 0.1f); // Adjust scale as needed
		if (_collisionShape.Shape is RectangleShape2D rectangleShape)
		{
			rectangleShape.Size = new Vector2(133.125f, 164f); // Adjust size to match the new scale
		}

		GD.Print("Sprite scale in Ready: ", _sprite.Scale);
	}

	private void OnBodyEntered(Node body)
	{
		if (canBeHit)
		{
			GD.Print("OnBodyEntered called with body: ", body.Name);
			GD.Print("Enemy hit by player");
			hitCount++;
			GD.Print("Hit count: ", hitCount);
			if (hitCount >= maxHits)
			{
				EmitSignal(nameof(EnemyDefeatedEventHandler));
				QueueFree(); // Remove enemy when defeated
			}
			else
			{
				ChangePosition(_boundaryRect);
			}

			canBeHit = false;
			_hitCooldownTimer.Start(); // Start the cooldown timer
		}
	}

	private void OnHitCooldownTimeout()
	{
		canBeHit = true; // Reset the ability to be hit after the cooldown
	}

	public void ChangePosition(Rect2 boundaryRect)
	{
		_boundaryRect = boundaryRect;
		Position = new Vector2(
			(float)GD.RandRange(boundaryRect.Position.X, boundaryRect.End.X),
			(float)GD.RandRange(boundaryRect.Position.Y, boundaryRect.End.Y)
		);
	}

	private void OnMovementTimeout()
	{
		ChangePosition(_boundaryRect);
	}

	public void SetPlayerAttacking(bool isAttacking)
	{
		// This method is not needed anymore as we handle the player attack state in Stage2 script
	}
}
