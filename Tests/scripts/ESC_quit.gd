extends Node

func _process(delta):
	if(Input.is_action_pressed("ui_cancel")): # ESC
		get_tree().quit();
