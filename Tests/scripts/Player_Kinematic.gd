extends KinematicBody

var gravity = -9.8
var velocity = Vector3()
var camera

var speed = 0;
var is_moving = false;
var character: Spatial;

const SPEED = 6
const ACCELERATION = 3
const DE_ACCELERATION = 5

func _ready():
	camera = get_node("../Camera").get_global_transform();
	
func _physics_process(delta):
	var dir = Vector3()
	is_moving = false;
	
	if(Input.is_action_pressed("ui_up")):
		dir += -camera.basis[2]
		is_moving = true;

	if(Input.is_action_pressed("ui_down")):
		dir += camera.basis[2]
		is_moving = true;

	if(Input.is_action_pressed("ui_left")):
		dir += -camera.basis[0]
		is_moving = true;

	if(Input.is_action_pressed("ui_right")):
		dir += camera.basis[0]
		is_moving = true;

	dir.y = 0
	dir = dir.normalized()

	if(dir==Vector3.ZERO):
		is_moving=false;

	velocity.y += delta * gravity

	var hv = velocity
	hv.y = 0

	var new_pos = dir * SPEED
	var accel = DE_ACCELERATION

	if (dir.dot(hv) > 0):
		accel = ACCELERATION

	hv = hv.linear_interpolate(new_pos, accel * delta)

	velocity.x = hv.x
	velocity.z = hv.z

	velocity = move_and_slide(velocity, Vector3(0,1,0))

	if (is_moving):
		var angle = atan2(hv.x, hv.z);
		angle += deg2rad(180);
		var char_rot = get_rotation();
		char_rot.y = angle;
		set_rotation(char_rot);

	speed = hv.length() / SPEED;

	var an = get_node_or_null("Node"); # anim node
	if(an!=null):
		an.isMoving = is_moving;
		an.speed = speed;
