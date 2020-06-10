using Godot;
using System;

public class Globals : Node
{
    //public static int Health=100;

    public static float Clamp(float x, float a, float b)
    {
        return x = (x < a) ? a : ((x > b) ? b : x);
    }

}
