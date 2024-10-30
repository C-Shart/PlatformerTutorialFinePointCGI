using Godot;
using System;

public partial class EventController : Node
{
    // [Signal] public delegate void LevelCompletedEventHandler();
    [Signal] public delegate void HeartCollectedEventHandler(int value);
    [Signal] public delegate void HeartLostEventHandler(int value);

    public static EventController Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
}
