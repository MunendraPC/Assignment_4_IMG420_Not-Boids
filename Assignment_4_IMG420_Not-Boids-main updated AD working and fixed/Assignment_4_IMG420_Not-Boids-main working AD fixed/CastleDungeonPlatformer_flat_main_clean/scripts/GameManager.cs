using Godot;

public partial class GameManager : Node
{
    public static GameManager I { get; private set; }

    public int Coins { get; private set; } = 0;

    public override void _EnterTree()
    {
        I = this;
    }

    public void AddCoin(int amount = 1)
    {
        Coins += amount;
        EmitSignal(SignalName.CoinsChanged, Coins);
    }

    [Signal] public delegate void CoinsChangedEventHandler(int newValue);
}
