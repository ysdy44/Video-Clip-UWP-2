using Microsoft.Graphics.Canvas;
using Video_Clip2.Clips.ClipManagers;
using Video_Clip2.Clips;
using Video_Clip2.Effects;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Effects;

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
                        if (item.Visibility == Visibility.Collapsed) continue;
                        if (item.Opacity == 0) continue;
                        if (item.Index != i) continue;

                        ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, this.PreviewCanvas, s.Size);
                        if (image == null) continue;

                        // Clip
                        ICanvasImage currentImage = Effect.Render(item.Effect, image, scale);

                        // Opacity
                        if (item.Opacity < 1)
                        {
                            args.DrawingSession.DrawImage(new OpacityEffect
                            {
                                Opacity = item.Opacity,
                                Source = currentImage
                            });
                        }
                        else
                        {
                            args.DrawingSession.DrawImage(currentImage);
                        }
                    }
                }
            };
        }

    }
}