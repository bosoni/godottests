//  cue stick
using System;
using Godot;

public class Stick : KinematicBody
{
	float mouseDX = 0, mouseDY = 0;
	bool mouseClicked = false;

	Vector2 lastPos;
	float LEN = 0.2f;
	float camRot = 0;

	Spatial cam, ball;
	Vector3 ballLastPos;
	Vector3 origBallPos;

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

	void UpdateMouse()
	{
		Vector2 pos = GetViewport().GetMousePosition();
		mouseDX = pos.x - lastPos.x;
		mouseDY = pos.y - lastPos.y;
		lastPos = pos;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//cam = GetNode<Spatial>("../Camera");
		ball = GetNode<Spatial>("../ballRigidBody");

		origBallPos = new Vector3(ball.Translation);
	}

	public override void _PhysicsProcess(float delta)
	{
		UpdateMouse();

		// mailaa ja kameraa pyöritetään ball:n ympärillä LEN matkan etäisyydellä
		if (mouseClicked == false)
		{
			camRot += mouseDX * delta;
			Vector3 r = new Vector3(0.2f, camRot, 0);
			SetRotation(r);
		}
		else // mouseclicked==true
		{
			LEN += mouseDY * delta;
			if (LEN < -0.2f) LEN = -0.2f;
			if (LEN > 2) LEN = 2;

			if (Globals.Moving)
				LEN = 2;

		}


		float cx = (float)(Math.Sin(camRot) * -LEN);
		float cz = (float)(Math.Cos(camRot) * -LEN);
		Vector3 v = new Vector3(ball.Translation);
		v.x += cx;
		v.z += cz;
		Translation = v;

		Vector3 diff = ballLastPos - ball.Translation;
		if (diff.Length() > 0.01f)
		{
			GD.Print(" moving.. ");
			Globals.Moving = true;
		}
		else
		{
			GD.Print(" moving = false ");
			Globals.Moving = false;
		}
		ballLastPos = ball.Translation;


		if (ball.Translation.y < 0.1f)
		{
			ball.Translation = origBallPos;
			Translation = origBallPos;
		}

	}
}
