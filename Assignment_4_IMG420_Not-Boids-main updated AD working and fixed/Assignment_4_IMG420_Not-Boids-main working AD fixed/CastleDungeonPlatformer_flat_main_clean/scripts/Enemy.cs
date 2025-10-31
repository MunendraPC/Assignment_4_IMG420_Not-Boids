using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 90f;
	[Export] public NodePath PlayerPath;
	private Node2D _player;
	private NavigationAgent2D _agent;
	private AnimatedSprite2D _anim;

	public override void _Ready()
	{
		_agent = GetNode<NavigationAgent2D>("Agent");
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_player = GetNodeOrNull<Node2D>(PlayerPath) ?? GetTree().CurrentScene.GetNode<Node2D>("Player");

		var tex = GD.Load<Texture2D>("res://assets/sprites/enemy.png");
		if (tex != null)
		{
			_anim.SpriteFrames = new SpriteFrames();
			_anim.SpriteFrames.AddAnimation("idle");
			_anim.SpriteFrames.AddAnimation("run");
			_anim.SpriteFrames.AddFrame("idle", tex);
			_anim.SpriteFrames.AddFrame("run", tex);
			_anim.Play("idle");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;

		_agent.TargetPosition = _player.GlobalPosition;

		Vector2 next = _agent.GetNextPathPosition();
		Vector2 dir = (next - GlobalPosition).Normalized();

		Velocity = dir * Speed;
		MoveAndSlide();

		if (Mathf.Abs(Velocity.X) > 1) _anim.FlipH = Velocity.X < 0;
		_anim.Play(Mathf.Abs(Velocity.X) > 2 ? "run" : "idle");
	}
}
