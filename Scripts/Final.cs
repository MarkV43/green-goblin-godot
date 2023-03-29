using Godot;
using System;

public partial class Final : Area2D
{
	public bool Occupied = false;
	public String TargetGroup = null;

	// I don't know how to explain why the function's name has to be like this... but it has to
	public virtual void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup(TargetGroup))
		{
			Occupied = true;
		}

		CheckWin();
	}

	public virtual void OnBodyExited(Node2D body)
	{
		GD.Print("Exited ", body);
		if (body.IsInGroup(TargetGroup))
		{
			Occupied = false;
		}
	}

	private void CheckWin()
	{
		var finals = GetTree().GetNodesInGroup("final");
		foreach (var node in finals)
		{
			if (!((Final)node).Occupied)
				return;
		}
		
		GD.Print("Won");
	}
}


/*
extends Area2D

class_name Final

var fbox_occupied = false
var fp_occupied = false
@onready var fb = get_tree().get_nodes_in_group("final_box").size()
@onready var fw = get_tree().get_nodes_in_group("final_weight").size()

func _on_body_entered(body):
	if is_in_group("final_box"):
		if body.is_in_group("box"):
			fbox_occupied = true
	if is_in_group("final_weight"):
		if body.is_in_group("weight"):
			fp_occupied = true		
	check_win()

func _on_body_exited(body):
	if is_in_group("final_box"):
		if body.is_in_group("box"):
			fbox_occupied -= 1
	if is_in_group("final_weight"):
		if body.is_in_group("weight"):
			fp_occupied = false
		
func check_win():
	var fboxes = get_tree().get_nodes_in_group("final_box")
	var fweights = get_tree().get_nodes_in_group("final_weight")
	var game_won = true
	
	for fbox in fboxes + fweights:
		if !fbox.fbox_occupied:
			game_won = false
			return
	
	print("win")
*/
