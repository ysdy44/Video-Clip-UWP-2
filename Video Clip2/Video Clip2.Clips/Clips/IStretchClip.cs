using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Clips
{
    public interface IStretchClip
    {
        Stretch Stretch { get; set; }
        CanvasBitmap Bitmap { get; }
        uint Width { get; }
        uint Height { get; }
        ICanvasImage Render(Size previewSize);
    }
}