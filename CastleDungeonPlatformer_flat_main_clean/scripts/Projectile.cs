using Godot;

public partial class Projectile : Area2D
{
    [Export] public float Speed = 360f;
    private Vector2 _dir = Vector2.Right;

    public void InitDirection(Vector2 dir) => _dir = dir.Normalized();

    public override void _Ready()
    {
        BodyEntered += OnBody;
        var t = GetNode<Timer>("Timer");
        t.Timeout += QueueFree;

        var tex = GD.Load<Texture2D>("res://assets/sprites/projectile.png");
        var spr = GetNodeOrNull<Sprite2D>("Sprite2D") ?? new Sprite2D();
        if (GetNodeOrNull<Sprite2D>("Sprite2D") == null) AddChild(spr);
        spr.Texture = tex;
    }

    public override void _Process(double delta)
    {
        GlobalPosition += _dir * Speed * (float)delta;
    }

    private void OnBody(Node body)
    {
        if (body.IsInGroup("Enemy"))
        {
            body.QueueFree();
            QueueFree();
        }
    }
}
