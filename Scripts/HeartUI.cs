using Godot;
using System;

public partial class HeartUI : Control
{
    public static HeartUI Instance { get; private set; }

    private Label label;
    // private EventController _eventController;

    public override void _Ready()
    {
        Instance = this;
        label = GetNode<Label>("Label");
        // _eventController = new EventController();
        // _eventController.Connect("HeartCollected", Callable.From(OnEventHeartCollected));
    }

    public void OnEventHeartCollected(int value)
    {
        label.Text = value.ToString();
    }
}
