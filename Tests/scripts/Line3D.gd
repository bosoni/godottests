# https://godotengine.org/qa/2762/draw-line-in-3d
extends Node2D

class Line:
	var id=0
	var color=Color(1,1,1)
	var thickness=1
	var a = Vector2()
	var b = Vector2()

var Lines
var Camera_Node

func _ready():
	Camera_Node = get_viewport().get_camera()

	Lines = []
	set_process(true)

func _draw():
	for line in Lines:
		draw_line(line.a, line.b, line.color, line.thickness)


var cc = 0;
var v1;
var v2;
var color = Color(0,1,1);
func _process(delta):
	if(Input.is_action_just_released("mouseLeft")):
		color = Color(randf(), randf(), randf());
		cc = 0;
		
	if(Input.is_action_pressed("mouseLeft")):
		var mpos = get_viewport().get_mouse_position();
		var origo = Vector2(get_viewport().size.x/2, get_viewport().size.y/2);
		var np = mpos - origo;
		mpos = np;
		
		mpos.x /= get_viewport().size.x;
		mpos.y /= get_viewport().size.y;

		mpos.x = (1 - mpos.x - 1) * 2;
		mpos.y *= 2 - 1
		
		print(">> "+str(mpos.x)+"  "+str(mpos.y));
		
		if(cc==0):
			v1 = mpos;
		elif(cc>=1):
			v2 = mpos;
			
			var _v1 = Vector3(v1.x, v1.y, 0.999999999);
			var _v2 = Vector3(v2.x, v2.y, 0.999999999);
			
			
			var l = (v2-v1).length();
			var thickness = 1 - l;
			if(thickness<0.01):
				thickness=0.01;
			thickness *= 10;
			DrawLine3D(cc, _v1, _v2 , color, thickness);
			v1 = v2;
			
			print("> "+str(thickness));
		
		cc+=1;

	update()

func DrawLine3D(id, vector_a, vector_b, color, thickness):
	"""
	for line in Lines:
		if line.id == id:
			line.color = color
			line.a = Camera_Node.unproject_position(vector_a)
			line.b = Camera_Node.unproject_position(vector_b)
			line.thickness = thickness
			return
	"""
	var new_line = Line.new()
	new_line.id = id
	new_line.color = color
	new_line.a = Camera_Node.unproject_position(vector_a)
	new_line.b = Camera_Node.unproject_position(vector_b)
	new_line.thickness = thickness
	Lines.append(new_line)

func Remove_Line(id):
	var i = 0
	var found = false
	for line in Lines:
		if line.id == id:
			found = true
			break
		i += 1
	if found:
		Lines.remove(i)
