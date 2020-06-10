extends Node

var animtree : AnimationTree;
var isMoving := false;
var speed = 0;

func _ready():
	animtree = get_node_or_null("../AnimationTree");

func _process(delta):
	var anim_to_play = "Idle";
	if(isMoving):
		anim_to_play = "Walk";

	if(animtree!=null):
		animtree.set("parameters/Blend2/blend_amount", speed);
		
		print("speed = ", speed);
