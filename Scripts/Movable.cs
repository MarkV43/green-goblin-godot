using Godot;
using System;
using Godot.Collections;

public partial class Movable : Node2D
{
	public const int GridSize = 32;

	public static readonly Dictionary<String, Vector2> Inputs = new()
	{
		{ "ui_up", Vector2.Up },
		{ "ui_down", Vector2.Down },
		{ "ui_left", Vector2.Left },
		{ "ui_right", Vector2.Right }
	};
	
	private RayCast2D Ray;
	public Sprite2D Sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// goblin = GetNode<Sprite2D>("Goblin");
		Ray = GetNode<RayCast2D>("RayCast2D");
		Sprite = GetNode<Sprite2D>("Sprite");
	}

	// Virtual means this function can be overwritten in other classes,
	// such as the Player
	public virtual bool CanMove(String dir)
	{
		var vector = Inputs[dir] * GridSize;
		Ray.TargetPosition = vector;
		Ray.ForceRaycastUpdate();
		if (Ray.IsColliding())
		{
			Node collider = Ray.GetCollider() as Node;
			// The ! serves to indicate that collider cannot be null.
			// This is needed because the .GetCollider() function can return null,
			// but we know for sure it will not be null because we already checked
			// with Ray.IsColliding() that the ray is in fact colliding with something
			if (collider!.IsInGroup("movable"))
			{
				return ((Movable)collider).CanMove(dir);
			}

			return false;
		}

		return true;
	}
	
	public virtual void Move(String dir)
	{
		if (CanMove(dir))
		{
			MoveUnchecked(dir);
		}
	}

	public void MoveUnchecked(String dir)
	{
		Movable coll = Ray.GetCollider() as Movable;
		// Instead of using an if statement, the ? serves the same purpose:
		// run the function only if the value is not null
		coll?.MoveUnchecked(dir);

		var vector = Inputs[dir] * GridSize;
		Position += vector;

		Animate(vector);
	}

	private void Animate(Vector2 vector)
	{
		CreateTween().TweenMethod(
			Callable.From<Vector2>(AnimationUpdate), 
			-vector,
			Vector2.Zero,
			0.07
		);
	}

	public virtual void AnimationUpdate(Vector2 vec)
	{
		Sprite.Position = vec;
	}
}
