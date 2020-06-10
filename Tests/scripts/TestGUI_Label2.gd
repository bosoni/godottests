extends Label

var txt = "Moro dorkki. Tää o tämmöne dork testi jou!";
var pos = 0;

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pos += delta*5;
	var t = txt.substr(0, pos);
	text = t;
	
	if(pos > txt.length()+10):
		pos=0;

	#print(" > ", pos, t); #debug
