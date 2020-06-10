using Godot;
using Godot.Collections;

public class Map : Spatial
{
	public float rayLength = 1000;
	Array excl = new Array();

	public void AddToMap(BaseObject obj, Node parent)
	{
		GlobalsCS.map.Add(obj);

		parent.AddChild(obj.ins);

		excl.Add(obj.ins);
	}

	public float GetY(float x, float z, bool toGround = true)
	{
		PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
		Dictionary result;
		var from = new Vector3(x, rayLength, z);
		var to = new Vector3(x, -rayLength, z);
		if (toGround)
			result = spaceState.IntersectRay(from, to, excl);
		else
			result = spaceState.IntersectRay(from, to);
		if (result.Count > 0)
			return ((Vector3)result["position"]).y;
		return 0;
	}
}
