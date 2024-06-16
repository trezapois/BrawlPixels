using Godot;
using System;

public partial class HealthBar : ProgressBar 
{
	[Export] private Timer timer;
	[Export] private ProgressBar damage_bar;
	private buddy HP;
	private int health;
	public int Health
	{
		get { return health; }
		set { SetHealth(value); }
	}

	public override void _Ready()
	{
		
		HP = GetNode<buddy>("/root/CharacterBody2D/buddy");
		health = 100;
		//GD.Print(health);
		timer = GetNode<Timer>("Timer");
		damage_bar = GetNode<ProgressBar>("DamageBar");
	}
	public override void _Process(double delta)
	{
//		HP = GetNode<buddy>("res://scenes/buddy");
//		health = HP.HP;
//		GD.Print(health);
//		SetHealth(health);
		
	}
	public void SetHealth(int newHealth)
	{
		int prev_health = health;
		health = Math.Min((int)MaxValue, newHealth);
		Value = health;

		if (health <= 0)
		{
			QueueFree();
		}

		if (health <= prev_health)
		{
			timer.Start();
		}
		else
		{
			damage_bar.Value = health;
		}
	}

	public void InitHealth(int _health)
	{
		health = _health;
		MaxValue = _health;
		damage_bar.MaxValue = _health;
		damage_bar.Value = _health;
	}


	private void _on_timer_timeout()
{
	damage_bar.Value = health;
	// Replace with function body.
}
}

