extends Node

var time = 0;
var dorks = [];
var count := 0;

func _process(delta):
	time+=delta*4;

	if(time>1):
		time=0;
		var tmp = preload("res://prefabs/DorkRB.tscn");
		var ins : RigidBody = tmp.instance();
		ins.set_translation(Vector3(rand_range(-2, 2), rand_range(2, 5), rand_range(-5, 5)));
		ins.set_scale(Vector3(0.2, 0.2, 0.2));

		randomColor(ins);

		# add label to model
		var label = Label.new();
		label.name = "Label";
		label.add_color_override("font_color", Color(randf(), randf(), randf()));
		label.text = ins.name + str(count);
		count+=1;
		ins.add_child(label);

		add_child(ins);

		dorks.push_back(ins);
		
	# update label position
	for d in dorks:
		var label = d.get_node("Label");
		var camera = d.get_node("../../Camera");
		var poss = d.get_translation();
		var offset = Vector2(label.get_size().x/2, 0);
		label.set_position(camera.unproject_position(poss) - offset);
		if(camera.is_position_behind(poss)):
			label.hide();
		else:
			label.show();


func randomColor(ins):
	var mat = SpatialMaterial.new();
	mat.albedo_color = Color(randf(), randf(), randf(), randf());

	var texname="";
	if(randf()<0.5):
		texname = "res://textures/bg1.png";
	else:
		texname = "res://textures/bg2.png";

	var tex = load(texname);
	mat.albedo_texture = tex;

	var m : MeshInstance = ins.find_node("Icosphere");
	m.material_override = mat;
