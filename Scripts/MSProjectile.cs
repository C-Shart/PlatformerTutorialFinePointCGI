using Godot;
using System;

public partial class MSProjectile : Node2D
{
    private int _speed = 150;
    private float _lifeSpan = 10f;

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Transform.X * (float)delta * _speed;
        _lifeSpan -= (float)delta;
        if(_lifeSpan <= 0){
            QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        QueueFree();
        if(body is PlayerController)
        {
            GameController.Instance.HeartLost(1);
        }
    }
}
