// https://www.youtube.com/watch?v=msZw59Iln74

using Godot;
using System;

public class Player_kinematic : KinematicBody
{
	float gravity = -9.8f;
	Vector3 velocity = new Vector3();
	Transform camera;
	AnimationPlayer anim_player;
	Spatial character;

	const float SPEED = 6, ACCELERATION = 3, DE_ACCELERATION = 5;

	public override void _Ready()
	{
		camera = GetNode<Camera>("../Camera").Transform;
		anim_player = GetNode<AnimationPlayer>("AnimationPlayer");
		character = GetNode<Spatial>(".");

	}

	public override void _PhysicsProcess(float delta)
	{
		var dir = new Vector3();
		bool is_moving = false;

		if (Input.IsActionPressed("ui_up"))
		{
			dir += -camera.basis[2];
			is_moving = true;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			dir += camera.basis[2];
			is_moving = true;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			dir += -camera.basis[0];
			is_moving = true;
		}
		if (Input.IsActionPressed("ui_right"))
		{
			dir += camera.basis[0];
			is_moving = true;
		}

		dir.y = 0;
		dir = dir.Normalized();

		//velocity.y += delta * gravity;

		var hv = velocity;
		hv.y = 0;

		var new_pos = dir * SPEED;
		var accel = DE_ACCELERATION;
		if (dir.Dot(hv) > 0)
			accel = ACCELERATION;

		hv = hv.LinearInterpolate(new_pos, accel * delta);

		velocity.x = hv.x;
		velocity.z = hv.z;

		velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0));

		var anim_to_play = "Idle";
		if (is_moving)
		{
			var angle = (float)Math.Atan2(hv.x, hv.z);
			angle += 3.1415f;
			var char_rot = character.Rotation;
			char_rot.y = angle;
			character.Rotation = char_rot;
			anim_to_play = "Walk";
		}

		var speed = hv.Length() / SPEED;

		var current_anim = anim_player.CurrentAnimation;
		if(current_anim != anim_to_play)
			anim_player.Play(anim_to_play);

	}
}
