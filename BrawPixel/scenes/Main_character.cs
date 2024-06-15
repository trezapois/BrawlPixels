using System;
using System.Collections.Generic;
using Godot;

namespace Test.scenes;

public abstract partial class Main_character : CharacterBody2D
{
	
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	/*
	private bool _inCombo = false;
	private double _timeTillNextImput = 0;
	private double _timeToCombo = 0;
	public Dictionary<List<int>, (string,int)> JlistInput { get; }
	public Dictionary<List<int>, (string,int)> KlistInput { get; }
	public List<int> MovementList { get; set; }
	public List<Attacks> AttacksList { get; set; }
	public Dictionary<List<Attacks>, (string,int)> JCombos { get; }
	public Dictionary<List<Attacks>, (string,int)> KCombos { get; }

	public void Jump()
	{
		velocity.Y = JumpVelocity;
	}
	
	public Main_character()
	{
		JlistInput = new Dictionary<List<int>, (string, int)>();
		KlistInput = new Dictionary<List<int>, (string, int)>();
		JCombos = new Dictionary<List<Attacks>, (string, int)>();
		KCombos = new Dictionary<List<Attacks>, (string, int)>();
	}
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	*/
	public static bool IsEqual(List<int> l1, List<int> l2)
	{
		if (l1.Count != l2.Count)
		{
			return false;
		}

		for (int i = 0; i < l1.Count; i++)
		{
			if (l1[i] != l2[i])
			{
				return false;
			}
		}

		return true;
	}
	
	public static bool IsEqual(List<Attacks> l1, List<Attacks> l2)
	{
		if (l1.Count != l2.Count)
		{
			return false;
		}

		for (int i = 0; i < l1.Count; i++)
		{
			if (l1[i] != l2[i])
			{
				return false;
			}
		}

		return true;
	}

	// convert string (used to call the animation) to attacks
	public static Attacks ToAttacks(string s)
	{
		if (s == "jab")
		{

			return Attacks.JAB1;
		}
		if (s == "double Jab")
		{
			return Attacks.JAB2;
		}
		if (s == "Heavy")
		{
			return Attacks.HEAVYATTACK;
		}
		if (s == "special_attack")
		{
			GD.Print("touch");
			return Attacks.SPECIAL_ATTACK;
		}
		// we will be adding more attacks when their animations will be created
		else
		{
			throw new Exception();
		}
	}
	/*
public override void _PhysicsProcess(double delta)
{
	Vector2 velocity = Velocity;

	// Add the gravity.
	if (!IsOnFloor())
		velocity.Y += gravity * (float)delta;

	// Handle Jump.
	if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		Jump();

	// Get the input direction and handle the movement/deceleration.
	// As good practice, you should replace UI actions with custom gameplay actions.
	Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
	if (direction != Vector2.Zero)
	{
		velocity.X = direction.X * Speed;
	}
	else
	{
		velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
	}

	Velocity = velocity;
	MoveAndSlide();
}
	 */
}
