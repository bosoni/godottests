/*
Gems  -puzzlepeli
(c) mjt, 2019
[mixut@hotmail.com]

Released under MIT license.

*/
using Godot;
using System;

public class Game : Spatial
{
	const int LKM = 10;
	int score = 0;
	PackedScene[] objs = new PackedScene[5];
	bool mouseClicked = false;

	StaticBody[] blocks = new StaticBody[5]; //  palikat

	StaticBody[,] inst = new StaticBody[LKM, LKM];

	int toBeDestroyed = 0;

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
				GetTree().Quit();
	}

	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates
		if (@event is InputEventMouseButton eventMouseButton)
		{
			mouseClicked = eventMouseButton.Pressed;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		GetNode<Button>("Button").Hide();

		Random rnd1 = new Random();

		for (int q = 0; q < 5; q++)
			objs[q] = (PackedScene)ResourceLoader.Load("res://prefabs/" + (q + 1) + ".res");

		float cc = -(float)LKM / 2.0f;
		for (int x = 0; x < LKM; x++)
		{
			for (int y = 0; y < LKM; y++)
			{
				int rr = (rnd1.Next() % 5); // randomilla model

				Vector3 pos = new Vector3(cc + (x * 1.1f) + 0.5f, 0, cc + (y * 1.1f));
				inst[x, y] = (StaticBody)objs[rr].Instance();

				inst[x, y].Name = "" + x + "" + y + "_" + rr;
				inst[x, y].SetTranslation(pos);
				AddChild(inst[x, y]);
			}
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		UpdateGems();
		GetNode<OmniLight>("OmniLight").Rotate(new Vector3(0, 1, 0), delta);
		GetNode<OmniLight>("OmniLight").Translate(new Vector3(0, 0, 2 * delta));

		if (!mouseClicked)
			return;
		mouseClicked = false;

		Vector2 mpos = GetViewport().GetMousePosition();
		var spaceState = GetWorld().DirectSpaceState;
		float rayLength = 1000;
		var camera = GetNode<Camera>("Camera");
		var from = camera.ProjectRayOrigin(mpos);
		var to = from + camera.ProjectRayNormal(mpos) * rayLength;
		Godot.Collections.Array excl = new Godot.Collections.Array();
		excl.Add(this);
		var result = spaceState.IntersectRay(from, to, excl);

		if (result.Count > 0)
		{
			Godot.Object o = (Godot.Object)result["collider"];
			if (o != null)
			{
				StaticBody sb = o as StaticBody;
				if (sb != null)
				{
					//GD.Print("DBG> "+sb.Name);
					CheckAndDestroy(sb.Name);
				}
			}
		}

	}

	int tmpCounter = 0;
	// tiputetaan gemssei alaspäin jos alempana on tyhjä
	void UpdateGems()
	{
		int x, y;
		for (x = 0; x < LKM; x++)
		{
			for (y = LKM - 1; y > 0; y--) // alhaalta ylös
			{
				if (inst[x, y] == null) // tyhjä paikka
				{
					inst[x, y] = inst[x, y - 1];
					inst[x, y - 1] = null;
					if (inst[x, y] != null)
					{
						inst[x, y].Translate(new Vector3(0, 0, 1.1f));
						tmpCounter = 0;
					}
				}
			}
		}

		if (tmpCounter > 50)
		{
			if (CheckIfMultipleGems() == false)
			{
				//GD.Print("  game over! ");
				GetNode<Button>("Button").Show();
			}
		}
		tmpCounter++;
	}

	void CheckAndDestroy(string name)
	{
		// eti x,y koordinaatit
		int x = -1, y = -1;
		bool found = false;
		for (x = 0; x < LKM; x++)
		{
			for (y = 0; y < LKM; y++)
			{
				if (inst[x, y] != null)
					if (inst[x, y].Name == name)
					{
						found = true;
						break;
					}
			}
			if (found)
				break;
		}
		if (!found || y == -1) return;

		// tarkista viereiset ruudut onko sama id (koska yksittäistä palikkaa ei tuhota)
		int id = GetId(name);
		toBeDestroyed = 0;
		if (GetId(x - 1, y) == id) toBeDestroyed++;
		if (GetId(x + 1, y) == id) toBeDestroyed++;
		if (GetId(x, y - 1) == id) toBeDestroyed++;
		if (GetId(x, y + 1) == id) toBeDestroyed++;
		if (toBeDestroyed >= 1)
		{
			FloodFill(x, y, id);
			GetNode<Label>("Label").SetText("Score: " + score);
			//GD.Print("Score: " + score);
		}
	}

	bool CheckIfMultipleGems()
	{
		int x, y;
		for (x = 0; x < LKM; x++)
		{
			for (y = 0; y < LKM; y++)
			{
				int id = GetId(x, y);
				if (id == -1) continue;
				if (GetId(x - 1, y) == id) return true;
				if (GetId(x + 1, y) == id) return true;
				if (GetId(x, y - 1) == id) return true;
				if (GetId(x, y + 1) == id) return true;
			}
		}
		return false;
	}

	int GetId(string name)
	{
		return int.Parse(name.Split("_")[1]);
	}
	int GetId(int x, int y)
	{
		if (x >= 0 && y >= 0 && x < LKM && y < LKM)
		{
			if (inst[x, y] == null) return -1;
			return int.Parse(inst[x, y].Name.Split("_")[1]);
		}
		return -1;
	}

	void FloodFill(int x, int y, int id)
	{
		if ((x < 0) || x >= LKM) return;
		if ((y < 0) || y >= LKM) return;
		if (GetId(x, y) == id)
		{
			inst[x, y].QueueFree();
			inst[x, y] = null;
			score++;
			FloodFill(x - 1, y, id);
			FloodFill(x + 1, y, id);
			FloodFill(x, y - 1, id);
			FloodFill(x, y + 1, id);
		}
	}

}
