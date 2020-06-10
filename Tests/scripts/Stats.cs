// http://docs.godotengine.org/en/stable/classes/class_performance.html?highlight=performance

using Godot;

public class Stats : Label
{
    bool debugEnabled = true;
 
    public override void _Process(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.Tab))
            debugEnabled = !debugEnabled;

        if (debugEnabled)
        {
            Show();
            var s1 = " FPS: " + (Performance.GetMonitor(Performance.Monitor.TimeFps)) + "\n";
            var s2 = " RENDER_OBJECTS_IN_FRAME: " + (Performance.GetMonitor(Performance.Monitor.RenderObjectsInFrame)) + "\n";
            var s3 = " RENDER_VERTICES_IN_FRAME: " + (Performance.GetMonitor(Performance.Monitor.RenderVerticesInFrame)) + "\n";
            var s4 = " RENDER_DRAW_CALLS_IN_FRAME: " + (Performance.GetMonitor(Performance.Monitor.RenderDrawCallsInFrame)) + "\n";
            var s5 = "  OBJECT_COUNT: " + (Performance.GetMonitor(Performance.Monitor.ObjectCount)) + "\n";
            var s6 = "  OBJECT_RESOURCE_COUNT: " + (Performance.GetMonitor(Performance.Monitor.ObjectResourceCount)) + "\n";
            var s7 = "  OBJECT_NODE_COUNT: " + (Performance.GetMonitor(Performance.Monitor.ObjectNodeCount)) + "\n\n";
            string outstr = s1 + s2 + s3 + s4 + s5 + s6 + s7;
            Text = outstr;
        }
        else
            Hide();
    }
}
