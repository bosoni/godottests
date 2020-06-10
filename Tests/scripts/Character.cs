using Godot;
using System;

class Character
{
	float MAX = 0.1f;
	Vector3 pos, rot, scale;
	Vector3 dir;
	Vector3 endPos;
	float speed = 1;
	
	string anim;
	int mode;
	
	public void WalkTo(Vector3 end)
	{
		endPos = end;
		dir = end - pos;
		//dir.Normalize(); TODO FIX
	}

	public void Update(float delta)
	{
		Vector3 l = endPos - pos;
		if(l.Length()<MAX)
		{
			pos = endPos;
			mode = 0;
			return;
		}
		
		pos += dir*delta*speed;
		mode = 1;
	
	}

}
