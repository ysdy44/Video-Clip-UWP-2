using Microsoft.Graphics.Canvas;

namespace Video_Clip2.Clips.ClipManagers
{
    public static partial class ClipManager
    {
        public static CanvasDevice CanvasDevice { get; } = new CanvasDevice();
    }
}