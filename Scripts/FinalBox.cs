using Godot;
using System;

public partial class FinalBox : Final
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		TargetGroup = "box";
	}
	
}
