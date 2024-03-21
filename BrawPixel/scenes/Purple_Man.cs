using Godot;
using System;
using System.Collections.Generic;

public partial class Purple_Man : CharacterBody2D
{
	
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private bool _inCombo = false;
	private double _timeTillNextImput = 0;
	private int _currentAttack = 0;
	private int _previousAttack = 0;
	public Dictionary<List<int>, string> JlistInput { get; }
	public Dictionary<List<int>, string> KlistInput { get; }
	public List<int> MovementList { get; set; }
	public List<Attacks> AttacksList { get; set; }
	public Dictionary<List<Attacks>, string> JCombos { get; }
	public Dictionary<List<Attacks>, string> KCombos { get; }

	Purple_Man()
	{
		JlistInput = new Dictionary<List<int>, string>();
		JlistInput.Add(new List<int>(){2,3,6},"heavy");
		KlistInput = new Dictionary<List<int>, string>();
		MovementList = new List<int>();
		AttacksList = new List<Attacks>();
		JCombos = new Dictionary<List<Attacks>, string>();
		JCombos.Add(new List<Attacks>() {Attacks.JAB1},"jab2");
		JCombos.Add(new List<Attacks>() { Attacks.JAB1 ,Attacks.JAB2},"jab3");
		KCombos = new Dictionary<List<Attacks>, string>();
	}


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


		if (!_inCombo)
		{
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			Console.WriteLine(direction.X);
			Console.WriteLine(direction.Y);
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
			// Clear the Movement list at neutral position
			if (direction.X == 0 && direction.Y == 0)
			{
				MovementList = new List<int>();
			}
			// Add a new Movement in the Movement list if different than the last element
			else if (direction.X == 0 && direction.Y > 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 8)
				{
					MovementList.Add(8);
				}
			}
			else if (direction.X > 0 && direction.Y > 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 9)
				{
					MovementList.Add(9);
				}
				
			}
			else if (direction.X < 0 && direction.Y > 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 7)
				{
					MovementList.Add(7);
				}
				
			}
			else if (direction.X > 0 && direction.Y == 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 6)
				{
					MovementList.Add(6);
				}			
			}
			else if (direction.X < 0 && direction.Y == 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 4)
				{
					MovementList.Add(4);
				}			
			}
			else if (direction.X == 0 && direction.Y < 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 2)
				{
					MovementList.Add(2);
				}
				
			}
			else if (direction.X < 0 && direction.Y < 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 1)
				{
					MovementList.Add(1);
				}
			}
			else if (direction.X > 0 && direction.Y < 0)
			{
				if (MovementList.Count == 0 || MovementList[^1] != 3)
				{
					MovementList.Add(3);
				}
			}
			
			
			if (Input.IsActionJustPressed("jab"))
			{
				try
				{
					SwitchAnimation(JlistInput[MovementList]);
				}
				catch
				{
					try
					{
						SwitchAnimation(JCombos[AttacksList]);
					}
					catch (Exception e)
					{
						SwitchAnimation("jab");
					}
					
				}
				_inCombo = true;
				_timeTillNextImput = 5;
			}
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
				velocity.Y = JumpVelocity;
		}
		else
		{
			velocity.X = 0;
			if(_timeTillNextImput > 0)
			{
				_timeTillNextImput = _timeTillNextImput - 0.1;
			}
			else
			{
				_inCombo = false;
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		
		
		
	}
	private void SwitchAnimation(string animationName)
	{
		animatedSprite.Animation = animationName;
	}

	
}
