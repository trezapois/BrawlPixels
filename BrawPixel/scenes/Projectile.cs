using Godot;

public partial class Projectile : Area2D
{
	[Export]
	public int Speed = 100; // Speed of the projectile
	[Export]
	public int Damage = 15; // Damage dealt by the projectile

	private Vector2 _direction = Vector2.Right; // Default direction
	private AnimatedSprite2D _animatedSprite; // Reference to the AnimatedSprite2D node

	public Vector2 knockback {get;set;} = new Vector2(0,0);
	public override void _Ready()
	{
		GD.Print("Projectile is ready");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play(); // Start the animation when the projectile is ready
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	public override void _Process(double delta)
	{
		MoveProjectile((float)delta);
	}

	private void MoveProjectile(float delta)
	{
		Position += _direction * Speed * delta; // Move in the specified direction
	}

	public void SetDirection(Vector2 direction)
	{
		_direction = direction.Normalized();
	}
	
	
	private void OnBodyEntered(Node2D body)
	{
		if(body is Purple_Man)
			((Purple_Man)body).handle_hit(Damage, knockback);
		if (body is buddy)
			((buddy)body).handle_hit(Damage, knockback);
		
		knockback = new Vector2(0,0);
		
	}
}
