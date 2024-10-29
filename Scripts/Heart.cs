using Godot;
using System;

public partial class Heart : Node2D
{
    [Export] public int value = 1;

    private AnimatedSprite2D _sprite;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _sprite.Play("idle");
    }

    private void _OnBodyEntered(Node2D body)
    {
        if(body is PlayerController){
            GameController.Instance.HeartCollected(value);
            _sprite.Play("collected");
            _animationPlayer.Play("collected");
        }
    }

    public void Destroy()
    {
        QueueFree();
    }
}
