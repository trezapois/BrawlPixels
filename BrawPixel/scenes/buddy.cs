using Godot;
using System;

public partial class buddy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public int HP = 100;
	
 [Signal]
public delegate void BuddyDiedEventHandler(); // Change delegate name to follow the convention



	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;
		MoveAndSlide();
	}
	public void handle_hit(int damage, Vector2 knockback)
	{
		HP -= damage;
		Console.WriteLine("1");
		if (HP <= 0)
        {
            HP = 0; // Ensure HP doesn't go negative
            Hide(); // Hide the buddy node when HP reaches 0
            EmitSignal(nameof(BuddyDied)); // Emit the signal when HP is zero
        }
	}
}
