using Godot;
using System;

public class ESC_quit : Node
{
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
				GetTree().Quit();
	}
}
