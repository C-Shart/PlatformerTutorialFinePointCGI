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

    public void HeartLost(int value)
    {
        bool updateUI = false;

        if(totalHearts != 0){
            updateUI = true;
        }
        if(totalHearts - value <= 0){
            totalHearts = 0;
        }
        else{
            totalHearts -= value;
        }

        GD.Print($"=======HEART LOSS=======");
        GD.Print($"Total   : {totalHearts}");
        GD.Print($"updateUI: {updateUI}");

        if (updateUI){
            EventController.Instance.EmitSignal("HeartLost", totalHearts);
            HeartUI.Instance.OnEventHeartLost(totalHearts);
        }
    }
}
