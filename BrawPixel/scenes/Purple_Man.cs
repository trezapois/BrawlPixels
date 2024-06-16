using Godot;
using System;
using System.Collections.Generic;

public partial class Purple_Man : Test.scenes.Main_character, IHittable
{
	[Signal]
	public delegate void CharacterDiedEventHandler(Purple_Man character);

	private bool _inCombo = false;
	private double _timeTillNextImput = 0;
	private int _currentAttack = 0;
	private int _previousAttack = 0;
	private double _timeToCombo = 0;

	public int hitstun { get; set; }
	public int HP { get; set; }
	private AnimationPlayer AP;
	public bool turned { get; set; }
	public int kbx { get; set; }
	public int kby { get; set; }

	public Dictionary<List<int>, (string, int)> JlistInput { get; }
	public Dictionary<List<int>, (string, int)> KlistInput { get; }
	public List<int> MovementList { get; set; }
	public List<Attacks> AttacksList { get; set; }
	public Dictionary<List<Attacks>, (string, int)> JCombos { get; }
	public Dictionary<List<Attacks>, (string, int)> KCombos { get; }

	private Vector2 syncPos = new Vector2(0, 0);
	private float syncRotation = 0;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D animatedSprite;
	private bool _playerAttacking = false;
	private Timer _attackTimer;

	public Purple_Man()
	{
		JlistInput = new Dictionary<List<int>, (string, int)>();
		JlistInput.Add(new List<int>() { 2, 3, 6 }, ("Heavy", 5));
		KlistInput = new Dictionary<List<int>, (string, int)>();
		MovementList = new List<int>();
		AttacksList = new List<Attacks>();
		JCombos = new Dictionary<List<Attacks>, (string, int)>();
		JCombos.Add(new List<Attacks>() { Attacks.JAB1 }, ("double Jab", 4));
		KCombos = new Dictionary<List<Attacks>, (string, int)>();
		HP = 100;
		turned = false;
		hitstun = 0;
	}

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		_attackTimer = new Timer();
		AddChild(_attackTimer);
		_attackTimer.WaitTime = 0.5f;
		_attackTimer.OneShot = true;
		_attackTimer.Connect("timeout", new Callable(this, nameof(OnAttackTimerTimeout)));

		if (int.TryParse(Name, out int authorityId))
		{
			GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(authorityId);
		}
		else
		{
			GD.PrintErr("Node name is not a valid authority ID: " + Name);
		}

		CollisionShape2D h = GetNode<Area2D>("Collider").GetNode<CollisionShape2D>("Hitbox");
		AP = GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		AP.Play("Idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		AP = GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		int temp = hitstun;

		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			hitstun = temp;
			Vector2 velocity = Velocity;

			if (_timeToCombo > 0)
				_timeToCombo -= 1;
			else
				AttacksList = new List<Attacks>();

			if (!IsOnFloor())
				velocity.Y += gravity * (float)delta;

			if (hitstun > 0)
			{
				velocity = new Vector2(200, -500);
				hitstun -= 1;
			}

			if ((AP.CurrentAnimation == "Idle" || AP.CurrentAnimation == "walk" || AP.CurrentAnimation == "getting hit") && hitstun == 0)
			{
				Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
				if (direction != Vector2.Zero)
				{
					bool movingLeft = direction.X < 0;
					velocity.X = direction.X * Speed;
					AP.Play("walk");
					animatedSprite.FlipH = movingLeft;
				}
				else
				{
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					AP.Play("Idle");
				}

				// Handle MovementList updates
				UpdateMovementList(direction);

				if (Input.IsActionJustPressed("punch"))
				{
					CollisionShape2D h = GetNode<Area2D>("Collider").GetNode<CollisionShape2D>("Hitbox");
					h.Disabled = false;

					foreach (var input in JlistInput.Keys)
					{
						if (IsEqual(input, MovementList))
						{
							(string action, int frames) = JlistInput[input];
							AP.Play(action);
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							((Collider)GetNode<Area2D>("Collider"))._setDam(20);
							GetNode<Collider>("Collider")._setK(new Vector2(500, -200));
							GetNode<Collider>("Collider")._setHitstun(10);
							return;
						}
					}

					foreach (var input in JCombos.Keys)
					{
						if (IsEqual(input, AttacksList))
						{
							(string action, int frames) = JCombos[input];
							AP.Play(action);
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							GetNode<Collider>("Collider")._setDam(10);
							GetNode<Collider>("Collider")._setK(new Vector2(-300, -200));
							GetNode<Collider>("Collider")._setHitstun(5);
							return;
						}
					}

					AP.Play("jab");
					_timeTillNextImput = 3;
					AttacksList.Add(Attacks.JAB1);
					GetNode<Collider>("Collider")._setDam(8);
					GetNode<Collider>("Collider")._setK(new Vector2(200, -200));
					GetNode<Collider>("Collider")._setHitstun(8);
					_timeToCombo = 50;
				}

				if (Input.IsActionJustPressed("kick"))
				{
					foreach (var input in KlistInput.Keys)
					{
						if (IsEqual(input, MovementList))
						{
							(string action, int frames) = KlistInput[input];
							AP.Play(action);
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							return;
						}
					}

					foreach (var input in KCombos.Keys)
					{
						if (IsEqual(input, AttacksList))
						{
							(string action, int frames) = JCombos[input];
							AP.Play(action);
							_timeTillNextImput = frames;
							AttacksList.Add(ToAttacks(action));
							_timeToCombo = frames + 50;
							return;
						}
					}

					AP.Play("Special");
					Special();
					_timeTillNextImput = 3;
					AttacksList.Add(Attacks.SMALLKICK);
					_timeToCombo = 50;
				}

				if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
					velocity.Y = JumpVelocity;
			}
			else if (AP.CurrentAnimation == "" && hitstun == 0)
			{
				AP.Play("Idle");
			}
			else if (hitstun == 0)
			{
				velocity.X = 0;
			}
			else
			{
				hitstun -= 1;
				_inCombo = true;
				velocity.Y = -500;
			}

			Velocity = velocity;
			_inCombo = false;
			MoveAndSlide();
			syncPos = GlobalPosition;
		}
	}

	private void UpdateMovementList(Vector2 direction)
	{
		// Update the movement list based on input direction
		MovementList.Add(direction.X > 0 ? 1 : direction.X < 0 ? -1 : 0);
		// Limit the size of the MovementList if needed
		if (MovementList.Count > 10)
		{
			MovementList.RemoveAt(0);
		}
	}

	public void handle_hit(int damage, Vector2 knockback, int stun)
	{
		HP -= damage;
		Velocity = new Vector2(-200, -200);
		hitstun = 500;
		if (HP <= 0)
		{
			EmitSignal(nameof(CharacterDied), this);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void SwitchAnimation(string animationName)
	{
		animatedSprite.Animation = animationName;
	}

	private void Special()
	{
	}

	private void FlipSprite(bool flip)
	{
		animatedSprite.FlipH = flip;
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			Rpc(nameof(SyncFlipState), flip);
		}

		var hitbox = GetNode<CollisionShape2D>("Collider/Hitbox");
		var collider = GetNode<Area2D>("Collider");
		var colliderShape = collider.GetNode<CollisionShape2D>("CollisionShape2D");

		ApplyFlip(hitbox, flip);
		ApplyFlip(colliderShape, flip);
	}

	private void ApplyFlip(CollisionShape2D shape, bool flip)
	{
		Vector2 scale = shape.Scale;
		scale.X = flip ? -Mathf.Abs(scale.X) : Mathf.Abs(scale.X);
		shape.Scale = scale;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SyncFlipState(bool flip)
	{
		animatedSprite.FlipH = flip;
	}

	public void handle_hit(int damage, Vector2 knockback)
	{
		HP -= damage;
	}

	private void OnAttackTimerTimeout()
	{
		_playerAttacking = false;
	}

	public bool IsAttacking()
	{
		return _playerAttacking;
	}

	public void SetAttacking(bool attacking)
	{
		_playerAttacking = attacking;
	}
}
