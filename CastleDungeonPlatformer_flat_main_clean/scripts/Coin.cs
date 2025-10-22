using Godot;

public partial class Coin : Area2D
{
	private GpuParticles2D _pickupFx;

	public override void _Ready()
	{
		_pickupFx = GetNode<GpuParticles2D>("PickupParticles");

		// Set sprite texture (simple single-frame coin) and make it spin via _Process
		var spr = GetNodeOrNull<Sprite2D>("Sprite2D"); 
		if (spr != null)
		{
			var tex = GD.Load<Texture2D>("res://assets/sprites/coin.png");
			spr.Texture = tex;
		}

		BodyEntered += OnBody;
	}

	public override void _Process(double delta)
	{
		var spr = GetNodeOrNull<Sprite2D>("Sprite2D"); 
		if (spr != null)
			spr.Rotation += 2.5f * (float)delta;
	}

	private async void OnBody(Node body)
	{
		if (body is CharacterBody2D)
		{
			GameManager.I.AddCoin(1);
			if (_pickupFx != null) _pickupFx.Emitting = true;
			var col = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
			if (col != null) col.Disabled = true;
			var spr = GetNodeOrNull<Sprite2D>("Sprite2D"); 
			if (spr != null) spr.Visible = false;

			await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
			QueueFree();
		}
	}
}
