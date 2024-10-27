using Godot;
using System;

public partial class GhostPlayer : Node2D
{
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
    }

    public void SetHValue(bool value)
    {
        GetNode<Sprite2D>("GhostFrame").FlipH = value;
    }

    public void Destroy()
    {
        QueueFree();
    }
}
