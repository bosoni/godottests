using Godot;
using System;

public class Test_benchmark : Spatial
{
	bool ok = false;
	RandomMap map;

	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		if (!ok)
		{
			var t1 = OS.GetTicksMsec();
			ok = true;
			map = new RandomMap();
			Utils.AddNode("ground", "res://prefabs/Ground2SB.tscn", GlobalsCS.root, map);
			var t2 = OS.GetTicksMsec();
			Utils.Log("time: "+ (t2-t1));
		}

		if (Input.IsKeyPressed((int)KeyList.Enter))
		{
			var t1 = OS.GetTicksMsec();
			for (int q = 0; q < 100; q++)
			{
				map.AddRandom(GetNode("/root/objs"));
			}
			var t2 = OS.GetTicksMsec();
			Utils.Log("time: "+ (t2-t1));
			Utils.Log("objs: " + GlobalsCS.map.Count);
		}

	}
}
