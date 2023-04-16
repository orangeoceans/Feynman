extends Node2D

var dragging = false
var drawing_line = false

var edge_scene = preload("res://src/scenes/edge.tscn")
	
func _process(delta):
	if dragging:
		var mousepos = get_viewport().get_mouse_position()
		self.position = Vector2(mousepos.x, mousepos.y)

func _set_drag_pc():
	dragging=!dragging


func _on_area2d_input_event(viewport, event, shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
			self.dragging = !self.dragging
		if event.button_index == MOUSE_BUTTON_RIGHT and event.pressed:
			self.drawing_line = !self.drawing_line
			var new_edge = edge_scene.instance()
	elif event is InputEventScreenTouch:
		if event.pressed and event.get_index() == 0:
			self.position = event.get_position()
		
