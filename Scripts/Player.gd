extends Movable

class_name Player

var moves = 0
var history = []
@onready var weight = get_tree().get_nodes_in_group('weight')[0]
var move_weight

func _ready():
	save_hist()

func _unhandled_input(event):
	for dir in inputs.keys():
		if event.is_action_pressed(dir):
			move(dir)
			return
	if event.is_action_pressed('undo'):
		undo()
		
func can_move(dir):
	if !super.can_move(dir):
		return false
		
	var vector = inputs[dir] * grid_size
	var new_position = position + vector
	var diff = (new_position - weight.position).abs()
	var distance = (diff.x + diff.y) / grid_size
	
	print('trying to move')
	
	if distance >= 4:
		if diff.x == 0 || diff.y == 0:
			if !move_weight:
				move_weight = true
				move_weight = weight.can_move(dir)
			return move_weight
		else:
			return false
	return true

func move(dir):
	move_weight = false
	if can_move(dir):
		if move_weight:
			weight.move_unchecked(dir)
		move_unchecked(dir)

func move_unchecked(dir):
	super.move_unchecked(dir)
	moves += 1
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
		
