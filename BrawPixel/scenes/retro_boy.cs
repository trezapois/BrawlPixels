using Godot;
using System;
using System.Collections.Generic;


public partial class Retro_boy : Test.scenes.Main_character,IHittable
{
	


	private bool _inCombo = false;
	private double _timeTillNextImput = 0;
	private int _currentAttack = 0;
	private int _previousAttack = 0;
	private double _timeToCombo = 0;
	private int hitstun = 0;
	public int HP {get;set;}
	
	public Dictionary<List<int>, (string,int)> JlistInput { get; }
	public Dictionary<List<int>, (string,int)> KlistInput { get; }
	public List<int> MovementList { get; set; }
	public List<Attacks> AttacksList { get; set; }
	public Dictionary<List<Attacks>, (string,int)> JCombos { get; }
	public Dictionary<List<Attacks>, (string,int)> KCombos { get; }

	Retro_boy()
	{
		JlistInput = new Dictionary<List<int>, (string,int)>();
		JlistInput.Add(new List<int>(){2,3,6},("Heavy",5));
		KlistInput = new Dictionary<List<int>, (string,int)>();
		MovementList = new List<int>();
		AttacksList = new List<Attacks>();
		JCombos = new Dictionary<List<Attacks>, (string,int)>();
		JCombos.Add(new List<Attacks>() {Attacks.JAB1},("double fast jab",4));
		//JCombos.Add(new List<Attacks>() { Attacks.JAB1 ,Attacks.JAB2},"jab3");
		KCombos = new Dictionary<List<Attacks>, (string,int)>();
		HP = 100;
	}
	
	
	private Vector2 syncPos = new Vector2(0,0);
	private float syncRotation = 0;
	

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()

	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		CollisionShape2D h = GetNode<Area2D>("Collider").GetNode<CollisionShape2D>("Hurtbox");
		h.Disabled = true;
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		if(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			Vector2 velocity = Velocity;
			if(_timeToCombo > 0)
				_timeToCombo -= 1;
			else
				AttacksList = new List<Attacks>();
			
			if (!IsOnFloor())
				velocity.Y += gravity * (float)delta;


			if (_inCombo == false && hitstun == 0)
			{
				Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
				if (direction != Vector2.Zero)
				{
					bool movingLeft = direction.X < 0;
					velocity.X = direction.X * Speed;
					SwitchAnimation("run");
	
					// Flip the sprite if moving left
					animatedSprite.FlipH = movingLeft;
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
				else if (direction.X == 0 && direction.Y < 0)
				{
					if (MovementList.Count == 0 || MovementList[^1] != 8)
					{
						MovementList.Add(8);
					}
				}
				else if (direction.X > 0 && direction.Y < 0)
				{
					if (MovementList.Count == 0 || MovementList[^1] != 9)
					{
						MovementList.Add(9);
					}
				
				}
				else if (direction.X < 0 && direction.Y < 0)
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
				else if (direction.X == 0 && direction.Y > 0)
				{
					if (MovementList.Count == 0 || MovementList[^1] != 2)
					{
						MovementList.Add(2);
					}
				
				}
				else if (direction.X < 0 && direction.Y > 0)
				{
					if (MovementList.Count == 0 || MovementList[^1] != 1)
					{
						MovementList.Add(1);
					}
				}
				else if (direction.X > 0 && direction.Y > 0)
				{
					if (MovementList.Count == 0 || MovementList[^1] != 3)
					{
						MovementList.Add(3);
					}
				}
				
		

			
				if (Input.IsActionJustPressed("punch"))
				{
					CollisionShape2D h = GetNode<Area2D>("Collider").GetNode<CollisionShape2D>("Hurtbox");
					h.Disabled = false;
					foreach (var input in JlistInput.Keys) 
					{
						if (IsEqual(input,MovementList)) 
						{ 
							(string action, int frames) = JlistInput[input];
							SwitchAnimation(action); 
							_inCombo = true;
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							((Collider)GetNode<Area2D>("Collider"))._setDam(20);
							return; 
						}
					}
					
					foreach (var input in JCombos.Keys)
					{
						if (IsEqual(input,AttacksList))
						{
							(string action, int frames) = JCombos[input];
							SwitchAnimation(action);
							_inCombo = true;
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							GetNode<Collider>("Collider")._setDam(10);
							return;
						}
					} 
					SwitchAnimation("jab");
					_inCombo = true;
					_timeTillNextImput = 3;
					AttacksList.Add(Attacks.JAB1);
					GetNode<Collider>("Collider")._setDam(8);
					_timeToCombo = 50;

				}
				if (Input.IsActionJustPressed("kick"))
				{
					foreach (var input in KlistInput.Keys) 
					{
						if (IsEqual(input,MovementList)) 
						{ 
							(string action, int frames) = KlistInput[input];
							SwitchAnimation(action); 
							_inCombo = true;
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							return; 
						}
					}
					
					foreach (var input in KCombos.Keys)
					{
						if (IsEqual(input,AttacksList))
						{
							(string action, int frames) = JCombos[input];
							SwitchAnimation(action);
							_inCombo = true;
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							return;
						}
					} 
					/*SwitchAnimation("kck");
					_inCombo = true;
					_timeTillNextImput = 3;
					AttacksList.Add(Attacks.SMALLKICK);
					_timeToCombo = 50;*/

				}
				if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
					velocity.Y = JumpVelocity;
			}
			else if(_inCombo)
			{
				velocity.X = 0;
				if(_timeTillNextImput > 0)
				{
					_timeTillNextImput = _timeTillNextImput - 0.1;
				}
				else
				{
					_inCombo = false;
					CollisionShape2D h = GetNode<Area2D>("Collider").GetNode<CollisionShape2D>("Hurtbox");
					h.Disabled = true;
				}
			}
			else
			{
				
			}

		Velocity = velocity;
		MoveAndSlide();
		syncPos = GlobalPosition;
		
		}
	}
	public void handle_hit(int damage, Vector2 knockback)
	{
		HP -= damage; 
		Velocity = new Vector2(-20,-20);
		hitstun = 20;
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void SwitchAnimation(string animationName)
	{
		animatedSprite.Animation = animationName;
	}


	 private void FlipSprite(bool flip)
	{
		animatedSprite.FlipH = flip; // flip character from left to right or vice versa
		GD.Print("Flip state changed to: ", flip);
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			Rpc(nameof(SyncFlipState), flip); // for the multiplayer
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SyncFlipState(bool flip)
	{
		animatedSprite.FlipH = flip;
		GD.Print("Synced flip state to: ", flip);
	}
	
	
	
}
