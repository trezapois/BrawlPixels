using Godot;
using System;

public partial class Collider : Area2D
{
	public int damage {get; set;} = 0;
	public Vector2 knockback {get;set;} = new Vector2(0,0);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _setDam(int n)
	{
		damage = n;
	}

	private void _on_body_entered(Node2D body)
	{
		if(body is Purple_Man)
			((Purple_Man)body).handle_hit(damage, knockback);
		if (body is buddy)
			((buddy)body).handle_hit(damage, knockback);
		damage = 0;
		knockback = new Vector2(0,0);
	}
}
