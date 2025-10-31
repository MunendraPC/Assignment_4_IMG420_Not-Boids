using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float MoveSpeed = 140f;
	[Export] public float JumpForce = 320f;
	[Export] public float Gravity = 900f;
	[Export] public float AirControl = 0.8f;
	[Export] public float DashSpeed = 360f;
	[Export] public float DashTime = 0.12f;
	[Export] public int MaxJumps = 2; // double jump
	[Export] public float WallSlideSpeed = 40f;

	private int _jumpsLeft;
	private bool _isDashing;
	private float _dashTimer;
	private int _facing = 1; // 1 right, -1 left

	private AnimatedSprite2D _anim;
	private GpuParticles2D _jumpParticles;
	private RayCast2D _wallCheck;
	private PackedScene _projectile;

	public override void _Ready()
	{
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_jumpParticles = GetNode<GpuParticles2D>("JumpParticles");
		_wallCheck = GetNode<RayCast2D>("WallCheck");
		_projectile = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
		_jumpsLeft = MaxJumps;

		// Basic sprite setup from placeholder textures
		var tex = GD.Load<Texture2D>("res://assets/sprites/player.png");
		if (tex != null)
		{
			_anim.SpriteFrames = new SpriteFrames();
			_anim.SpriteFrames.AddAnimation("idle");
			_anim.SpriteFrames.AddFrame("idle", tex);
			_anim.Play("idle");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var v = Velocity;

		// Gravity
		if (!_isDashing && !IsOnFloor())
			v.Y += Gravity * (float)delta;

		// Horizontal input
		float input = 0f;
		if (Input.IsActionPressed("move_left")) input -= 1f;
		if (Input.IsActionPressed("move_right")) input += 1f;

		if (!_isDashing)
		{
			float accel = IsOnFloor() ? 1f : AirControl;
			v.X = Mathf.Lerp(v.X, input * MoveSpeed, accel);
		}

		// Facing & crouch
		if (input != 0) _facing = (int)Mathf.Sign(input);
		bool crouch = Input.IsActionPressed("crouch") && IsOnFloor();

		// Jump logic (ground + double)
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				v.Y = -JumpForce;
				_jumpsLeft = MaxJumps - 1;
				BurstJumpParticles();
			}
			else if (_jumpsLeft > 0)
			{
				v.Y = -JumpForce;
				_jumpsLeft--;
				BurstJumpParticles();
			}
		}

		if (IsOnFloor()) _jumpsLeft = MaxJumps;

		// Wall slide (if moving into wall in air)
		bool againstWall = _wallCheck.IsColliding() && !IsOnFloor() && input == _facing;
		if (againstWall && v.Y > WallSlideSpeed)
			v.Y = WallSlideSpeed;

		// Dash (short burst, no gravity)
		if (!_isDashing && Input.IsActionJustPressed("dash"))
		{
			_isDashing = true;
			_dashTimer = DashTime;
			v = new Vector2(_facing * DashSpeed, 0);
		}
		if (_isDashing)
		{
			_dashTimer -= (float)delta;
			if (_dashTimer <= 0f)
				_isDashing = false;
		}

		// Fire projectile
		if (Input.IsActionJustPressed("fire"))
			Fire();

		Velocity = v;
		MoveAndSlide();

		UpdateAnimation(input, crouch);
		FlipSprite();
		UpdateWallRay();
	}

	private void BurstJumpParticles()
	{
		_jumpParticles.Emitting = false;
		_jumpParticles.Emitting = true; // one-shot burst
	}

	private void Fire()
	{
		if (_projectile == null) return;
		var p = _projectile.Instantiate<Area2D>();
		GetTree().CurrentScene.AddChild(p);
		p.GlobalPosition = GlobalPosition + new Vector2(_facing * 12, -6);
		p.Call("InitDirection", new Vector2(_facing, 0)); // simple API
	}

	private void UpdateAnimation(float input, bool crouch)
	{
		if (crouch && IsOnFloor()) { _anim.Play("idle"); return; }
		if (!IsOnFloor())
			_anim.Play(Velocity.Y < 0 ? "idle" : "idle");
		else if (Mathf.Abs(input) > 0.1f)
			_anim.Play("idle");
		else
			_anim.Play("idle");
	}

	private void FlipSprite()
	{
		_anim.FlipH = _facing < 0;
	}

	private void UpdateWallRay()
	{
		_wallCheck.TargetPosition = new Vector2(_facing * 8, 0);
	}
}
