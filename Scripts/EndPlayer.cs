using Godot;
using System;

public partial class EndPlayer : End
{
    public override void _Ready()
    {
        base._Ready();
        TargetGroup = "player";
    }
}