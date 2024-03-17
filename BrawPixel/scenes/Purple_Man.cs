using Godot;
using System;

public partial class Purple_Man : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

	
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			SwitchAnimation("run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			SwitchAnimation("Idle");
		}

		Velocity = velocity;
		MoveAndSlide();

		if (Input.IsActionJustPressed("jab"))
        {
            SwitchAnimation("jab");
        }
	}
	private void SwitchAnimation(string animationName)
    {
        animatedSprite.Animation = animationName;
    }

	
}
