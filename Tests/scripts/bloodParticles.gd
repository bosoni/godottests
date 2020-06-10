extends Particles

func _ready():
	get_node("Timer").connect("timeout", self, "dispose");
	set_emitting(true);
	
func dispose():
	#print("   debug dispose blood particles");
	queue_free();
