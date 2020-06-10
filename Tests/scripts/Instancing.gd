extends Node

var time = 0;

func _process(delta):
	time+=delta*4;
	
	if(time>1):
		time=0;
		var tmp = preload("res://prefabs/DorkRB.tscn");
		var ins : RigidBody = tmp.instance();
		ins.set_translation(Vector3(rand_range(-2, 2), rand_range(2, 5), rand_range(-2, 2)));
		ins.set_scale(Vector3(0.2, 0.2, 0.2));

		randomColors(ins);

		add_child(ins);

func randomColors(ins):
	var mat = SpatialMaterial.new();
	mat.albedo_color = Color(randf(), randf(), randf(), randf());
	#mat.params_blend_mode = SpatialMaterial.BLEND_MODE_ADD;

	var texname="";
	if(randf()<0.5):
		texname = "res://textures/bg1.png";
	else:
		texname = "res://textures/bg2.png";

	var tex = load(texname);
	mat.albedo_texture = tex;

	var m : MeshInstance = ins.find_node("Icosphere");
	m.material_override = mat;
