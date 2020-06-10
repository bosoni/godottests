extends Node

var anim_player : AnimationPlayer;
var isMoving := false;
var speed = 0;

func _ready():
	# jos modelilla animationtree, poista se (muuten ei animaatiot näy)
	# (mutta jos modelilla on animationtree niin olis parempi käyttää AnimBlendingController skriptiä)
	var animtree = get_node_or_null("../AnimationTree");
	if(animtree!=null):
		get_node("../AnimationTree").queue_free();

	anim_player = get_node("../UkkoArmature/AnimationPlayer");

func _process(delta):
	var anim_to_play = "Idle";
	if(isMoving):
		anim_to_play = "Walk";
		
	var current_anim = anim_player.get_current_animation();
	if(current_anim != anim_to_play):
		anim_player.play(anim_to_play);
