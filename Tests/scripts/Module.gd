class_name Module

static func add(name, parent, scriptName="", sceneName="", scriptToModel=false):
	#print("  debug add "+name);
	
	var node = Node.new();
	node.name = name;
	var ins;
	
	if(sceneName != ""):
		var model = load(sceneName);
		ins = model.instance();
		node.add_child(ins);

	if(scriptName != ""):
		var script = load(scriptName).new();
		script.name = "script";
		if(scriptToModel):
			if(sceneName==""):
				print("   debug: no model loaded but scriptToModel==true");
				node.add_child(script);
			else:
				ins.add_child(script);
		else:
			node.add_child(script);

	parent.add_child(node);

	return node;

static func remove(node):
	#print("  debug release "+node.name);
	node.queue_free();
