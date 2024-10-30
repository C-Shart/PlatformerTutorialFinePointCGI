using Godot;
using System;

public partial class HeartUI : Control
{
    public static HeartUI Instance { get; private set; }

    private Label label;

    public override void _Ready()
    {
        Instance = this;
        label = GetNode<Label>("Label");
    }

    public void OnEventHeartCollected(int value)
    {
        GD.Print("Heart gained!");
        label.Text = value.ToString();
    }

    public void OnEventHeartLost(int value)
    {
        GD.Print("Heart lost!");
        GD.Print($"New value: {value}");
        label.Text = value.ToString();
    }
}
