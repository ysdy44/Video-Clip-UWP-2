using Microsoft.Graphics.Canvas;
using Video_Clip2.Clips.ClipManagers;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Effects;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructPreview()
        {
            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                double height = e.NewSize.Height;
                this.PreviewGrid.SetWindowHeight(height - 48);
            };
            this.FullScreenButton.Click += (s, e) =>
            {
                double height = base.ActualHeight;
                this.Animation.To = this.PreviewGrid.FullScreenStarted(height - 48);
                this.Storyboard.Begin();
            };
            this.Storyboard.Completed += (s, e) => this.PreviewGrid.FullScreenCompleted();

            this.PreviewCanvas.UseSharedDevice = true;
            this.PreviewCanvas.CustomDevice = ClipManager.CanvasDevice;
            this.PreviewCanvas.Draw += (s, args) =>
            {
                float scale = (float)this.ViewModel.Scale;
                int rows = this.RectangleCanvas.MaximumRows;

                for (int i = 0; i < rows; i++)
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