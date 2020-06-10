using Godot;
using System;

public class Button : Godot.Button
{
    public void _on_Button_pressed()
    {
        //GD.Print("halloota");
        GetTree().ChangeScene("res://game.tscn");
    }

}
