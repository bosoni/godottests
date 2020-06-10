extends Spatial

var MUL = 50;

func _ready():
	generateTerrain("res://terrain/terrain.png", "res://terrain/terrain.material");

# luodaan terrain kolmioista
func generateTerrain(heightmapImage, heighmapShader):
	#var img = Image.new();
	#img.load(heightmapImage);
	var image = load(heightmapImage);
	var img = image.get_data();
	
	var st = SurfaceTool.new();
	
	print("debug: generating...");
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	img.lock();
	for z in img.get_height()-1:
		for x in img.get_width()-1:
			var y1 = img.get_pixel(x, z).r * MUL;
			var y2 = img.get_pixel(x+1, z).r * MUL;
			var y3 = img.get_pixel(x+1, z+1).r * MUL;
			#st.add_uv(Vector2(0, 0))
			st.add_vertex(Vector3(x, y1, z))
			#st.add_uv(Vector2(0, 1))
			st.add_vertex(Vector3(x+1, y2, z))
			#st.add_uv(Vector2(1, 1))
			st.add_vertex(Vector3(x+1, y3, z+1))
	
			y1 = img.get_pixel(x, z).r * MUL;
			y2 = img.get_pixel(x+1, z+1).r * MUL;
			y3 = img.get_pixel(x, z+1).r * MUL;
			#st.add_uv(Vector2(0, 0))
			st.add_vertex(Vector3(x, y1, z))
			#st.add_uv(Vector2(0, 1))
			st.add_vertex(Vector3(x+1, y2, z+1))
			#st.add_uv(Vector2(1, 1))
			st.add_vertex(Vector3(x, y3, z+1))

	img.unlock();
	print("debug: generating normals...");
	
	st.generate_normals();

	var mesh_instance = MeshInstance.new() 
	mesh_instance.mesh = st.commit() 
	#mesh_instance.set_surface_material(0, load(heighmapShader)); todo
	mesh_instance.create_trimesh_collision();
	add_child(mesh_instance) 

func _process(delta):
	rotate_y(delta*0.5);
