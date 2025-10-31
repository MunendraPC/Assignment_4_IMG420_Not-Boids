using Godot;

public partial class GameUI : Control
{
	private Label _coinLabel;

	public override void _Ready()
	{
		_coinLabel = GetNode<Label>("CoinLabel");
		_coinLabel.Text = "Coins: 0";
		GameManager.I.CoinsChanged += OnCoinsChanged;
	}

	private void OnCoinsChanged(int value)
	{
		_coinLabel.Text = $"Coins: {value}";
	}
}
