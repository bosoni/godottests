using Godot;
using Godot.Collections;

public class Utils : Node
{
	static Dictionary<string, PackedScene> modelCache = new Dictionary<string, PackedScene>();

	public static void Log(string txt)
	{
		GD.Print(txt);
	}

	public static Spatial LoadModel(string sceneName, bool visible = true)
	{
		PackedScene model;
		if (modelCache.ContainsKey(sceneName))
		{
			model = modelCache[sceneName];
		}
		else
		{
			Log("loadmodel " + sceneName);
			model = GD.Load<PackedScene>(sceneName);
			modelCache[sceneName] = model;
		}
		//Log("  return instance");
		var ins = (Spatial)model.Instance();
		ins.Visible = visible;
		return ins;
	}

	public static void CreateMultiMesh(string sceneName, Vector3[] poss, Vector3[] rots=null, Vector3[] scales=null)
	{
		var  s = LoadModel(sceneName);
		s.Visible = false;

		for(int q=0;q<poss.Length;q++)
		{
// TODO

		}

/*

	# create a simple box mesh
	var mesh = CubeMesh.new()
	mesh.size = Vector3(1,1,1)

	# create multi mesh
	var multiMesh = MultiMesh.new()
	multiMesh.transform_format = MultiMesh.TRANSFORM_3D
	multiMesh.color_format = MultiMesh.COLOR_FLOAT
	multiMesh.mesh = mesh
	multiMesh.instance_count = 4

	var mmi = MultiMeshInstance.new()
	mmi.multimesh = multiMesh;

	# create collision shape
	var collisionNode = StaticBody.new()
	var collisionShape = CollisionShape.new()
	collisionShape.shape = multiMesh.mesh.create_trimesh_shape()
	collisionNode.add_child(collisionShape)
	mmi.add_child(collisionNode)
	add_child(mmi)

	for mesh_index in range(multiMesh.instance_count):
		var position = Transform().translated(Vector3(mesh_index*2, 10+2, 0)).scaled(Vector3(1,1,1))
		multiMesh.set_instance_transform(mesh_index, position)

*/

	}

	public static Node AddNode(string newNodeName, string sceneName, Node parent, Spatial script = null)
	{
		if(parent!=null)
			Log("node " + newNodeName + " -> parent " + parent.Name);
		else Log("node " + newNodeName + " -> root");
		
		var node = new Node();
		node.Name = newNodeName;

		Node ins = LoadModel(sceneName);
		node.AddChild(ins);

		if (script != null)
			node.AddChild(script);

		parent.AddChild(node);
		//parent.CallDeferred("AddChild", node);

		return node;
	}

	public float Raycast(Spatial scene, Vector3 from, Vector3 to, Array excl=null)
	{
		PhysicsDirectSpaceState spaceState = scene.GetWorld().DirectSpaceState;
		Dictionary result;
		if (excl!=null)
			result = spaceState.IntersectRay(from, to, excl);
		else
			result = spaceState.IntersectRay(from, to);

		if (result.Count > 0)
			return ((Vector3)result["position"]).y;

		return 0;

		/*
		{
		   position: Vector3 # point in world space for collision
		   normal: Vector3 # normal in world space for collision
		   collider: Object # Object collided or null (if unassociated)
		   collider_id: ObjectID # Object it collided against
		   rid: RID # RID it collided against
		   shape: int # shape index of collider
		   metadata: Variant() # metadata of collider
		}	
		*/
	}

	public void ChangeTexture(Spatial mesh, string name, string newTextureName, Color newColor)
	{
		var mi = mesh.FindNode(name);
		MeshInstance m = (MeshInstance)mi;

		SpatialMaterial mat = new SpatialMaterial();
		var texture = (Texture)GD.Load(newTextureName);
		mat.AlbedoTexture = texture;
		mat.AlbedoColor = newColor;

		m.MaterialOverride = mat;
	}

	// ===========================================================================
	static int rcounter =0;
	const int UPDATE_OBJECTS=2; // monenko framen välein päivitetään objektit (tsekataan näkyykö vai ei)
	const int MAXLEN=10*10; // len^2  kuinka kaukana olevat objektit piilotetaan
	Array<Spatial> childs=new Array<Spatial>(); // temp taulukko objekteille
	public void RenderAllObjects()
	{
		var gn = GetNode("/root/objs");
		foreach(Spatial p in childs)
		{
			gn.AddChild(p);
			childs.Remove(p);
		}
		foreach(Spatial p in gn.GetChildren())
			p.Show();
	}

	// piilottaa kaukana ja kameran takana olevat obut
	public void RenderOnlyNearObjects(Vector3 playerPos, Camera camera)
	{
		rcounter+=1;
		// ei tsekata joka framella
		if(rcounter >= UPDATE_OBJECTS) rcounter=0;
		else return;

		/*
		jos objekti liian kaukana kamerasta tai kameran takana,
		laita se childs taulukkoon ja poista se scenestä.
		tsekkaa kaik obut childs taulukosta onko lähellä ja kameran edessä,
		jos on, lisää takas sceneen.
		*/
		var gn = GetNode("/root/objs");
		foreach(Spatial p in gn.GetChildren())
		{
				var l = p.Translation - playerPos;
				var llen = l.LengthSquared();
				if(llen >= MAXLEN || camera.IsPositionBehind(p.Translation))
				{
					childs.Add(p);
					gn.RemoveChild(p);
				}
		}

		foreach(Spatial p in childs)
		{
				var l2 = p.Translation - playerPos;
				var llen2 = l2.LengthSquared();
				if(llen2 < MAXLEN && !camera.IsPositionBehind(p.Translation))
				{
					gn.AddChild(p);
					childs.Remove(p);
				}
		}        
	}

	// vaihtaa obun lod-modelin(jos sellane on) etäisyyden mukaan
	public static void UseLOD(string path, Camera camera)
	{
		// TODO
	}

	public static void CreateCollisionShape()
	{
		// TODO
		/*
		* collisionshape (KB, RB, SB)  koodilla?  Utils.CreateCollisionShape()

		http://docs.godotengine.org/en/3.2/classes/class_meshinstance.html
		create_convex_collision(), create_trimesh_collision()
		
		add_shape()
		http://docs.godotengine.org/en/stable/classes/class_shape.html#class-shape

			var body = RigidBody.new()
			body.add_child(TestCube.new())
			var shape = BoxShape.new()
			body.add_shape(shape)
			body.set_translation(Vector3(0,5,0))
			add_child(body) 
		*/
	}

}
