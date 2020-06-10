using Godot;
using System;

public class Instancing : Node
{
	PackedScene obj;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		obj = (PackedScene)ResourceLoader.Load("res://prefabs/rock.tscn.res");
		Random rnd1 = new Random();

		for (int q = 0; q < 500; q++)
		{
			int x = (rnd1.Next() % 20) - 10;
			int z = (rnd1.Next() % 20) - 10;

			int y = (rnd1.Next() % 60) + 10;

			RigidBody inst = (RigidBody)obj.Instance();
			inst.Translation = new Vector3(x, y, z);
			AddChild(inst);
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
				GetTree().Quit();
	}
}
