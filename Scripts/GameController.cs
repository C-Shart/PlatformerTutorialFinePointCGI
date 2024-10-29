using Godot;
using System;

public partial class GameController : Node
{
    public static GameController Instance { get; private set; }

    private int totalHearts = 0;

    public override void _Ready()
    {
        Instance = this;
    }

    public void HeartCollected(int value){
        totalHearts += value;
        EventController.Instance.EmitSignal("HeartCollected", totalHearts);
        HeartUI.Instance.OnEventHeartCollected(totalHearts);
    }
}
