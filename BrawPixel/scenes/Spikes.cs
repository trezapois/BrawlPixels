using Godot;
using System;

public partial class Spikes : Node
{
	
	public CharacterBody2D owner {get; set;}
	public int Lifespan {get;set;}

	Spikes(CharacterBody2D o)
	{
		owner = o;
		Lifespan = 20;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Lifespan == 0)
		{
			QueueFree();
		}
		else
		{
			Lifespan --;
		}
	}
	
	public void _on_body_entered(Area2D body)
	{
		/*
		if(body != owner)
		{
			if(body is Purple_Man)
				((Purple_Man)body).handle_hit(10, new Vector2(0.0),0);
			if(body is retro_boy)
				((retro_boy)body).handle_hit(10, new Vector2(0.0),0);
			//if(body is Purple_Man)
				//((Purple_Man)body).handle_hit(10, new Vector2(0.0),0);
			if (body is buddy)
				((buddy)body).handle_hit(10, new Vector2(0.0),0);
			
		}
		*/
	}
	
}


