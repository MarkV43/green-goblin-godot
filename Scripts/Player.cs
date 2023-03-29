using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Player : Movable
{
	public static int Moves = 0;
	// We'll use a stack here for performance
	private static readonly Stack<Array<Vector2>> History = new();
	private Movable _weight;
	private bool _moveWeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		_weight = GetTree().GetNodesInGroup("weight")[0] as Movable;
		SaveHist();
	}
	
	// The @ serves the sole purpose of enabling you to use reserved keywords as a variable name.
	// For example, you can't name a variable `int` because that's the name of a type,
	// but you can name it @int.
	public override void _UnhandledInput(InputEvent @event)
	{
		foreach (var dir in Inputs.Keys)
		{
			if (@event.IsActionPressed(dir))
			{
				Move(dir);
				return;
			}
		}

		if (@event.IsActionPressed("undo"))
		{
			Undo();
		}
	}

	public override bool CanMove(String dir)
	{
		// `base` is the equivalent to `super` in python or gdscript
		if (!base.CanMove(dir))
			return false;

		var vector = Inputs[dir] * GridSize;
		var newPosition = Position + vector;
		var diff = (newPosition - _weight.Position).Abs();
		var distance = (diff.X + diff.Y) / GridSize;
		
		GD.Print("Trying to move");
		
		// I refactored it a bit, but the logic is the exact same

		if (distance < 4)
			return true;

		if (diff.X != 0 && diff.Y != 0)
			return false;

		if (_moveWeight) return _moveWeight;
		
		_moveWeight = true;
		_moveWeight = _weight.CanMove(dir);
		return _moveWeight;
	}

	public override void Move(String dir)
	{
		_moveWeight = false;
		if (!CanMove(dir))
			return;

		if (_moveWeight)
			_weight.MoveUnchecked(dir);
		
		MoveUnchecked(dir);
		
		// I moved the logic from MoveUnchecked to here
		Moves += 1;
		SaveHist();
	}

	private void SaveHist()
	{
		var movs = GetTree().GetNodesInGroup("movable");
		Array<Vector2> state = new();
		foreach (var node in movs)
		{
			var movable = (Node2D) node;
			state.Add(movable.Position);
		}
		
		History.Push(state);
	}

	private void Undo()
	{
		if (History.Count <= 1)
			return;

		var movs = GetTree().GetNodesInGroup("movable");
		History.Pop();
		var state = History.Peek();
		for (int i = 0; i < movs.Count; i++)
		{
			((Node2D) movs[i]).Position = state[i];
		}
	}
}
