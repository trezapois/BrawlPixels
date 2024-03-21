using Godot;
using System;

public partial class Punching_ball : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();



	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
	{
		//animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;



		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void handle_hit()
	{
		Console.WriteLine("OUCH");
	}
}
