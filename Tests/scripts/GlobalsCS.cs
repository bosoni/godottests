using Godot;
using Godot.Collections;

public class GlobalsCS : Node
{
	public static Array<BaseObject> map = new Array<BaseObject>();
	public static RandomNumberGenerator rnd = new RandomNumberGenerator();
	public static Node root;
	public static Node ui;
	public static WorldEnvironment env; // WorldEnvironment node	
	
	/**
		 settings["nicewater"]	normalmaps, liike
		 settings["lights"]		ellei päällä, pitäis käydä koko scene läpi ja disabloida valot
		 settings["shadows"]
		 settings["ssao"]
		 settings["reflection"]

		 TODO
	*/
	public static Dictionary<string, bool> settings = new Dictionary<string, bool>();

	public override void _Ready()
	{
		root = GetNode("/root");
	}

}
