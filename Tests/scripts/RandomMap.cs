using Godot;

public class RandomMap : Spatial
{
	static string[] models = { "Tree1SB", "Tree2SB", "RockSB", "Mushroom2SB" };
	public int SIZE = 40;
	public int OBJS = 1000;
	public Map map = new Map();

	public RandomMap()
	{
		Name = "randommap";
	}

	public override void _Ready()
	{
		Init();
	}

	public void Init()
	{
		Utils.Log("randommap " + OBJS);
		var n = new Node();
		n.Name = "objs";
		GetNode("/root").AddChild(n);

		for (int q = 0; q < OBJS; q++)
		{
			AddRandom(n);
		}
	}

	public void AddRandom(Node parent)
	{
		float x = GlobalsCS.rnd.Randf() * SIZE - SIZE / 2;
		float z = GlobalsCS.rnd.Randf() * SIZE - SIZE / 2;
		float y = map.GetY(x, z);
		if (y <= 0) return; // ei veden alle

		BaseObject bo = new BaseObject();
		int i = GlobalsCS.rnd.RandiRange(0, 3);
		bo.ins = Utils.LoadModel("res://prefabs/" + models[i] + ".tscn");

		bo.ins.Translation = new Vector3(x, y, z);
		bo.ins.Rotation = new Vector3(0, GlobalsCS.rnd.Randf() * 100, 0);
		if (i == 3) bo.ins.Scale = new Vector3(0.1f, 0.1f, 0.1f);

		map.AddToMap(bo, parent);
	}

}
