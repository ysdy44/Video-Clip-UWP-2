using Microsoft.Graphics.Canvas;
using Video_Clip2.Clips.ClipManagers;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructPreview()
        {
            this.PreviewCanvas.UseSharedDevice = true;
            this.PreviewCanvas.CustomDevice = ClipManager.CanvasDevice;
            this.PreviewCanvas.Draw += (s, args) =>
            {
                float scale = (float)this.ViewModel.Scale;

                for (int i = 0; i < 5; i++)
                {
                    foreach (IClip item in this.ViewModel.ObservableCollection)
                    {
                        if (item.Visibility == Visibility.Visible)
                        {
                            if (item.Index == i)
                            {
                                ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, this.PreviewCanvas, s.Size);

                                if (image != null)
                                {
                                    args.DrawingSession.DrawImage(Effect.Render(item.Effect, image, scale));
                                }
                            }
                        }
                    }
                }
            };
        }

    }
}