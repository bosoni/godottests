/*
Memmu
(c) mjt, 2019
[mixut@hotmail.com]

Released under MIT license.

*/
using Godot;
using System;

public class Game : Spatial
{
    const int LKM = 4;
    int tries = 0, destroyed = 0;
    PackedScene obj;
    bool mouseClicked = false;

    /* mode määrää mitä tapahtuu:
     *   0 voi valita ekan obun
     *   1 käännä eka palikka
     *   2 käännä toka palikka
     *   3 eri palikat -> käännä palikat takaisin -> mode=0
     *   4 samat palikat -> pyöritä palikoita -> mode=5
     *   5 poista palikat -> mode=0
     */
    int mode = 0;
    StaticBody[] selectedBlock = new StaticBody[2]; // kummatkin palikat
    bool turn = false; // onko palikka kääntymässä

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
            //GD.Print("Mouse Click/Unclick at: ", eventMouseButton.Position);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tries = 0;
        destroyed = 0;
        GetNode<Button>("Button").Hide();

        obj = (PackedScene)ResourceLoader.Load("res://prefabs/block.res");
        Random rnd1 = new Random();

        Spatial s = GetNode<Spatial>("cornerPos");

        int[] imgs = new int[8];
        for (int q = 0; q < 8; q++) imgs[q] = 0;

        for (int x = 0; x < LKM; x++)
        {
            for (int y = 0; y < LKM; y++)
            {
                int rr = -1;
                while (true)
                {
                    rr = (rnd1.Next() % 8); // randomilla kuva
                    if (imgs[rr] < 2) // pöydällä vain 2 samaa kuvaa
                    {
                        imgs[rr]++;
                        break;
                    }
                }

                Vector3 pos = new Vector3(s.GetTranslation().x + x * 2.3f, 0, s.GetTranslation().z + y * 2.3f);
                StaticBody inst = (StaticBody)obj.Instance();

                Material m = ResourceLoader.Load("res://models/" + (rr + 1) + ".material") as Material;
                MeshInstance tmp = inst.GetNode<MeshInstance>("imagePlane");
                tmp.MaterialOverride = m;

                inst.Name = "" + x + "" + y + "_" + rr;
                inst.SetTranslation(pos);
                AddChild(inst);
            }
        }
    }

    float wait = 0;
    float ang = 0;
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (turn == true)
        {
            delta *= 8;

            Vector3 v = new Vector3(1, 0, 1).Normalized();

            if (mode == 1) // käännä eka palikka
            {
                selectedBlock[0].Rotate(v, delta);
                ang += delta;

                if (ang >= 3.14)
                {
                    ang = 0;
                    turn = false;
                }
            }
            if (mode == 2) // käännä toka palikka
            {
                selectedBlock[1].Rotate(v, delta);
                ang += delta;

                if (ang >= 3.14)
                {
                    //GD.Print("DBG> " + selectedBlock[0].Name + "  " + selectedBlock[1].Name);

                    // onko sama kuva
                    if (selectedBlock[0].Name.Split("_")[1] == selectedBlock[1].Name.Split("_")[1])
                    {
                        mode = 4; // samat
                    }
                    else
                    {
                        mode = 3; // eri
                        wait = 0;
                    }
                    ang = 0;
                }
            }
            if (mode == 3) // käännä palikat takaisin
            {
                wait += delta;
                if (wait < 3)
                    return;

                selectedBlock[0].Rotate(v, -delta);
                selectedBlock[1].Rotate(v, -delta);
                ang += delta;
                if (ang >= 3.14)
                {
                    turn = false;
                    mode = 0;
                    ang = 0;
                    tries++;
                    GetNode<Label>("triesLabel").SetText("Tries: " + tries);
                    //GD.Print("t: " + tries);
                }
            }

            if (mode == 4) // samat kuvat, hävitä palikat
            {
                selectedBlock[0].Rotate(v, ang);
                selectedBlock[1].Rotate(v, ang);
                ang += 2 * delta;
                if (ang > 10)
                {
                    ang = 0;
                    mode = 5;
                }
            }
            if (mode == 5)
            {
                selectedBlock[0].QueueFree();
                selectedBlock[1].QueueFree();
                turn = false;
                mode = 0;
                destroyed += 2;
                if (destroyed == LKM * LKM)
                {
                    GetNode<Button>("Button").Show();
                }

            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (turn == true)
            return;
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
        excl.Add(this); // this=Collision exception ettei tsekata obun omaan collision areaan
        var result = spaceState.IntersectRay(from, to, excl);

        if (result.Count > 0)
        {
            Godot.Object o = (Godot.Object)result["collider"];
            if (o != null)
            {
                StaticBody sb = o as StaticBody;
                if (sb != null)
                {
                    if (mode > 0)
                    {
                        // tarkista ettei paineta samaa obua 2 kertaa
                        if (selectedBlock[0].Name == sb.Name)
                            return;
                    }

                    selectedBlock[mode] = sb;

                    turn = true;
                    mode++;
                }
            }
        }
    }
}
