using Godot;
using System;

public partial class InterfaceManager : CanvasLayer
{
    public static DialogueManager dialogueManager;
    public override void _Ready()
    {
        dialogueManager = GetNode("DialogueManager") as DialogueManager;
    }

}
