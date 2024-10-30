using Godot;
using System;

public partial class HeartUI : Control
{
    public static HeartUI Instance { get; private set; }

    private Label _label;

    public override void _Ready()
    {
        Instance = this;
        _label = GetNode<Label>("Label");
        _label.Text = GameController.Instance.StartHealth.ToString();
    }

    public void OnEventHeartCollected(int value)
    {
        GD.Print("Heart gained!");
        _label.Text = value.ToString();
    }

    public void OnEventHeartLost(int value)
    {
        GD.Print("Heart lost!");
        GD.Print($"New value: {value}");
        _label.Text = value.ToString();
    }
}
