using Godot;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		EnsureInputs();          // make sure actions exist even if project.godot didn't load them
		BuildFlatLevel();        // paint a long, flat floor so the player doesn't fall
	}

	private void EnsureInputs()
	{
		void Add(string name, Key key)
		{
			if (!InputMap.HasAction(name))
			{
				InputMap.AddAction(name);
				var ev = new InputEventKey { Keycode = key };
				InputMap.ActionAddEvent(name, ev);
			}
		}
		Add("move_left", Key.A);
		Add("move_right", Key.D);
		Add("jump", Key.Space);
		Add("dash", Key.Shift);
		Add("crouch", Key.S);
		Add("fire", Key.F);
	}

	/// <summary>
	/// Paints a simple, long floor row at y=2 tiles (64px), using layer 0 of the TileMap.
	/// </summary>
	private void BuildFlatLevel()
	{
		var tm = GetNodeOrNull<TileMap>("TileMap");
		if (tm == null) return;

		// Clear existing
		tm.ClearLayer(0);

		// Paint floor across a wide range
		for (int x = -20; x < 200; x++)
			tm.SetCell(0, new Vector2I(x, 3), 0, new Vector2I(0, 0));
	}
}
