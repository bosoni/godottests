extends Spatial


func _ready():
	blood(Vector3(0,0,0));
	fire(Vector3(0,0,0));

func _process(delta):
	pass

func blood(pos):
	var blood = preload("res://prefabs/bloodParticles.tscn");
	var ins : Particles = blood.instance();
	ins.set_translation(pos);
	var SC = 3; #0.5;
	ins.set_scale(Vector3(SC, SC, SC));
	#get_node("..").add_child(ins);
	add_child(ins);
	
	#ins.set_emitting(true);
	

func fire(pos):
	var fr = preload("res://prefabs/fireParticles.tscn");
	var ins : Particles = fr.instance();
	ins.set_translation(pos);
	#var SC = 3; #0.5;
	#ins.set_scale(Vector3(SC, SC, SC));
	#get_node("..").add_child(ins);
	add_child(ins);
		
	#ins.set_emitting(true);
		
	
