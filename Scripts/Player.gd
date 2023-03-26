extends CharacterBody2D

var ray
var grid_size = 32
var inputs = {
	'ui_up': Vector2.UP,
	'ui_down': Vector2.DOWN,
	'ui_left': Vector2.LEFT,
	'ui_right': Vector2.RIGHT,
}
var moves = 0
var history = []

func _ready():
	ray = $RayCast2D
	save_hist()

func _unhandled_input(event):
	for dir in inputs.keys():
		if event.is_action_pressed(dir):
			move(dir)
			return
	if event.is_action_pressed('undo'):
		undo()

func move(dir):
	var vector = inputs[dir] * grid_size
	ray.target_position = vector
	ray.force_raycast_update()
	if !ray.is_colliding():
		move_unchecked(vector)
	else:
		var collider = ray.get_collider()
		if collider.is_in_group('movable'):
			if collider.move(dir):
				move_unchecked(vector)

func move_unchecked(vec):
	position += vec
	moves += 1
	print(moves)
	save_hist()
	
func save_hist():
	var movs = get_tree().get_nodes_in_group('movable')
	var state = []
	for movable in movs:
		state.push_back(movable.position)
	history.push_back(state)

func undo():
	if len(history) <= 1:
		return
	var movs = get_tree().get_nodes_in_group('movable')
	history.pop_back()
	var state = history[len(history)-1]
	for i in range(len(state)):
		print(i)
		movs[i].position = state[i]
		
