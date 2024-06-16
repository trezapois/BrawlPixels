using Godot;
using System;

public partial class buddy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public int HP = 100;
	private int hitstun = 0;
	private AnimationPlayer AP;
	private int moving = 0;
	private bool jumping = false;
	
	[Signal]
	public delegate void BuddyDiedEventHandler(); // Change delegate name to follow the convention
 
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private static Random _random = new Random();

	public static int GetRandomNumber()
	{
		return _random.Next(0, 101);
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		AP = GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		Vector2 velocity = Velocity;
		if(hitstun > 0)
			hitstun --;
		else
		{
			if(jumping)
			{
				velocity.Y = JumpVelocity;
				jumping = false;
			}
			if(moving > 0)
				velocity.X = -200;
			else if(hitstun == 0)
				velocity.X = 0;
		}
		// Add the gravity.
			
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;
		if(velocity.X > 0)
			AP.Play("walk");
		MoveAndSlide();
		GD.Print(velocity);
	}
	
	public void AImove(int dist)
	{
		AP = GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		int r = GetRandomNumber();
		r = r * dist / 250;
		if(r > 50)
		{
			moving = 100;
		}
		else
		{
			moving = 0;
		}
	}
	
	public void AIaction(int oppHp,int dist)
	{
		AP = GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		int r = GetRandomNumber();
		if(dist < 500)
		{
			if(r%10 > 6)
			{
				if(oppHp < 25)
				{
					r=r/2;
				}
				if(r > 50)
				{
					((Collider)GetNode<Area2D>("Coll"))._setDam(20);
					AP.Play("heavy");
				}
				else
				{
					((Collider)GetNode<Area2D>("Coll"))._setDam(8);
					AP.Play("jab");
				}
			}
		}
		if(r % 10 < 4)
		{
			jumping = true;
		}
	}
	
	public void handle_hit(int damage, Vector2 knockback, int stun)
	{
		HP -= damage; 
		Velocity = new Vector2(200,-200);
		hitstun = stun * 3;
		GD.Print(HP);
		GD.Print(stun);
		if (HP <= 0)
		{
			HP = 0; // Ensure HP doesn't go negative
			Hide(); // Hide the buddy node when HP reaches 0
			EmitSignal(nameof(BuddyDied)); // Emit the signal when HP is zero
		}
	}
}
