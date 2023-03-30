using Godot;
using System;

public partial class EndWeight : End
{
    public override void _Ready()
    {
        base._Ready();
        TargetGroup = "weight";
    }
}