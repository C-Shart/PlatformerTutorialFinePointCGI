using Godot;
using System;

public partial class DialogueManager : Control
{

    public override void _Ready()
    {
        
    }

    public void ShowDialogue()
    {
        GetNode<PanelContainer>("PanelContainer");  // Paused here -- Need to dive deeper into Popup doc
    }
}