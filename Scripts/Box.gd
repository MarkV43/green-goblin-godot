extends CharacterBody2D

var ray
var grid_size = 32
var inputs = {
	'ui_up': Vector2.UP,
	'ui_down': Vector2.DOWN,
	'ui_left': Vector2.LEFT,
	'ui_right': Vector2.RIGHT,
}

func _ready():
	ray = $RayCast2D

func move(dir):
	var vector = inputs[dir] * grid_size
	ray.target_position = vector
	ray.force_raycast_update()
	if !ray.is_colliding():
		position += vector
		return true
	return false
