using Godot;
using System;

public class Test_SSAO : Node
{
	public override void _Ready()
	{
		GlobalsCS.env = GlobalsCS.root.GetNodeOrNull<WorldEnvironment>("Test_SSAO/WorldEnvironment");
		
	}

	public override void _Process(float delta)
	{
		if(Input.IsActionJustReleased("f1"))
		{
			GlobalsCS.env.Environment.SsaoEnabled = !GlobalsCS.env.Environment.SsaoEnabled;
			Utils.Log("ssao " + GlobalsCS.env.Environment.SsaoEnabled);
		}
		if(Input.IsActionJustReleased("f2"))
		{
			GlobalsCS.env.Environment.FogEnabled = !GlobalsCS.env.Environment.FogEnabled;
			Utils.Log("fog " + GlobalsCS.env.Environment.FogEnabled);
		}	

	}
}
