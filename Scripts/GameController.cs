using Godot;
using System;

public partial class GameController : Node
{
    public static GameController Instance { get; private set; }

    [Signal] public delegate void DeathEventHandler();

    [Export] public Marker2D RespawnPoint;
    [Export] public int StartHealth;

    private int _totalHearts = 4;

    public override void _Ready()
    {
        Instance = this;
        StartHealth = _totalHearts;

        RespawnPoint = GetNode<Marker2D>("/root/Node2D/RespawnPoint");
    }

    public void HeartCollected(int value){
        _totalHearts += value;
        EventController.Instance.EmitSignal("HeartCollected", _totalHearts);
        HeartUI.Instance.OnEventHeartCollected(_totalHearts);

    }

    public void HeartLost(int value)
    {
        bool updateUI = false;

        if(_totalHearts != 0){
            updateUI = true;
        }
        if(_totalHearts - value <= 0){
            updateUI = true;
            _totalHearts = 0;
            // Player dies logic:
            //  Play death animation
            //  Reset hearts value
            //  Temporarily remove player control
            //  Emit Death Signal
            EmitSignal(nameof(Death));
            //  Respawn character:
            RespawnPlayer();
        }
        else{
            _totalHearts -= value;
        }

        if (updateUI){
            EventController.Instance.EmitSignal("HeartLost", _totalHearts);
            HeartUI.Instance.OnEventHeartLost(_totalHearts);
        }
    }

    public void RespawnPlayer()
    {
        // I know this breaks encapsulation oh well:
        PlayerController pc = GetNode<PlayerController>("/root/Node2D/PlayerCeleste");
        pc.GlobalPosition = RespawnPoint.GlobalPosition;
        pc.Respawn();
    }

    private void OnPlayerDeath()
    {
        RespawnPlayer();
    }
}