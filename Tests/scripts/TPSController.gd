# https://github.com/Sombresonge/Third-Person-Controller
extends Spatial

export(NodePath) var PlayerPath  = "" #You must specify this in the inspector!
export(float) var MovementSpeed = 10
export(float) var Acceleration = 3
export(float) var MaxJump = 10
export(float) var MouseSensitivity = 8
export(float) var RotationLimit = 45
export(float) var MaxZoom = 0.5
export(float) var MinZoom = 1.5
export(float) var ZoomSpeed = 2

var Player
var InnerGimbal
var Direction = Vector3()
var Rotation = Vector2()
var gravity = -10
var Movement = Vector3()
var ZoomFactor = 1
var ActualZoom = 1
var Speed = Vector3()
var CurrentVerticalSpeed = Vector3()
var JumpAcceleration = 3
var IsAirborne = false
var is_moving = false;

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	Player = get_node(PlayerPath)
	InnerGimbal = $InnerGimbal
	pass

func _unhandled_input(event):
	if event is InputEventMouseMotion :
		Rotation = event.relative
	
	if event is InputEventMouseButton:
		match event.button_index:
			BUTTON_WHEEL_UP:
				ZoomFactor -= 0.05
			BUTTON_WHEEL_DOWN:
				ZoomFactor += 0.05
		ZoomFactor = clamp(ZoomFactor, MaxZoom, MinZoom)

func _physics_process(delta):
	Direction = Vector3();
	is_moving = false;
	
	if(Input.is_action_pressed("ui_up")):
		Direction.z -= 1
		is_moving = true;
	if(Input.is_action_pressed("ui_down")):
		Direction.z += 1
		is_moving = true;
	if(Input.is_action_pressed("ui_left")):
		Direction.x -= 1
		is_moving = true;
	if(Input.is_action_pressed("ui_right")):
		Direction.x += 1
		is_moving = true;
	if(Input.is_action_pressed("ui_select")):
		if not IsAirborne:
			CurrentVerticalSpeed = Vector3(0,MaxJump,0)
			IsAirborne = true

	
	if(is_moving==false):
		#Movement *= 0.2;
		#Direction *= 0.2;
		Speed *= 0.9;
		
	Direction.z = clamp(Direction.z, -1,1)
	Direction.x = clamp(Direction.x, -1,1)
	
	#Rotation
	Player.rotate_y(deg2rad(-Rotation.x)*delta*MouseSensitivity)
	InnerGimbal.rotate_x(deg2rad(-Rotation.y)*delta*MouseSensitivity)
	InnerGimbal.rotation_degrees.x = clamp(InnerGimbal.rotation_degrees.x, -RotationLimit, RotationLimit)
	Rotation = Vector2()
	
	#Movement
	var MaxSpeed = MovementSpeed *Direction.normalized()
	Speed = Speed.linear_interpolate(MaxSpeed, delta * Acceleration)
	Movement = Player.transform.basis * (Speed)
	CurrentVerticalSpeed.y += gravity * delta * JumpAcceleration
	Movement += CurrentVerticalSpeed
		
	if Player.is_on_floor() :
		CurrentVerticalSpeed.y = 0
		IsAirborne = false
	
	#Zoom
	ActualZoom = lerp(ActualZoom, ZoomFactor, delta * ZoomSpeed)
	InnerGimbal.set_scale(Vector3(ActualZoom,ActualZoom,ActualZoom))
	Player.move_and_slide(Movement,Vector3(0,1,0))

	var an = get_node_or_null("../Node"); # anim node
	if(an!=null):
		an.isMoving = is_moving;
		an.speed = Speed.length();

	
	#https://docs.godotengine.org/en/3.1/classes/class_kinematicbody.html#class-kinematicbody-method-move-and-collide
	#KinematicCollision move_and_collide ( Vector3 rel_vec, bool infinite_inertia=true, bool exclude_raycast_shapes=true, bool test_only=false )
	var col = Player.move_and_collide(Movement * 0.01);
	if(col != null):
		var gc = col.get_collider();
		var cname = gc.name;
		if(cname.find("Plane") == -1):
			print(cname);
			gc.queue_free();

