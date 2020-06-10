extends Spatial

var firstTime = true;

func _physics_process(delta):
	if(firstTime==false):
		return;
	firstTime = false;
	createTrees();

func createTrees():
	var positions = [];
	var space_state = get_world().direct_space_state
	
	# random paikka kartalla
	var SIZE = 8;
	for c in range(50):
		var x = randf() * SIZE - SIZE/2;
		var z = randf() * SIZE - SIZE/2;
		
		var from = Vector3(x, 100, z);
		var to = Vector3(x, -100, z);

		var result = space_state.intersect_ray(from, to, [self]);
		positions.push_back(result.position);
		
	# pistä paikkoihin puu/sieni
	for p in positions:
		var model;
		if(randf()<0.8):
			model = preload("res://prefabs/Tree1SB.tscn");
		else:
			model = preload("res://prefabs/MushroomSB.tscn");
			
		var ins : StaticBody = model.instance();
		ins.set_translation(p);
		add_child(ins);
