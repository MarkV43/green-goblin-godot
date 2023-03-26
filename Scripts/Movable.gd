extends Node2D

class_name Movable 

@onready var ray = $RayCast2D
var grid_size = 32
var inputs = {
	'ui_up': Vector2.UP,
	'ui_down': Vector2.DOWN,
	'ui_left': Vector2.LEFT,
	'ui_right': Vector2.RIGHT,
}

func can_move(dir):
	var vector = inputs[dir] * grid_size
	ray.target_position = vector
	ray.force_raycast_update()
	if ray.is_colliding():
		var collider = ray.get_collider()
		if collider.is_in_group('movable'):
			return collider.can_move(dir)
		return false
	return true

func move(dir):
	if can_move(dir):
		move_unchecked(dir)
	
func move_unchecked(dir):
	var coll = ray.get_collider()
	if coll != null:
		coll.move_unchecked(dir)
		
	var vector = inputs[dir] * grid_size
	position += vector
