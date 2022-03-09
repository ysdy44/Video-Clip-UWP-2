using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Video_Clip2.Clips;
using Video_Clip2.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructPreview()
        {
            this.PlayButton.Click += (s, e) =>
            {
                if (this.ViewModel.Position >= this.ViewModel.Duration)
                    this.ViewModel.Position = TimeSpan.Zero;

                this.ViewModel.IsPlaying = true;
                this.PlayRing.Ding();
                this.PauseButton.Focus(FocusState.Programmatic);
            };
            this.PauseButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;
                this.PlayRing.Ding();
                this.PlayButton.Focus(FocusState.Programmatic);
            };



            this.PreviewCanvas.UseSharedDevice = true;
            this.PreviewCanvas.CustomDevice = ClipManager.CanvasDevice;
            this.PreviewCanvas.Draw += (s, args) =>
            {
                float scale = (float)this.ViewModel.Scale;
                int rows = this.RectangleCanvas.MaximumRows;
                switch (this.AppBarListView.SelectedIndex)
                {
                    case 0:
                        this.DurationMenu.Draw(args.DrawingSession, s.Size);
                        break;

                    default:
                        for (int i = 0; i < rows; i++)
                        {
                            Matrix3x2 matrix = Matrix3x2.CreateScale(scale);
                            foreach (Clipping item in this.ViewModel.ObservableCollection)
                            {
                                IClip clip = item.Self;

                                if (clip.Visibility == Visibility.Collapsed) continue;
                                if (clip.Opacity == 0) continue;
                                if (clip.Index != i) continue;

                                ICanvasImage image = clip.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, matrix);
                                if (image == null) continue;

                                // Clip
                                ICanvasImage currentImage = Effect.Render(clip.Effect, image, scale);

                                // Opacity
                                if (clip.Opacity < 1)
                                {
                                    args.DrawingSession.DrawImage(new OpacityEffect
                                    {
                                        Opacity = clip.Opacity,
                                        Source = currentImage
                                    });
                                }
                                else
                                {
                                    args.DrawingSession.DrawImage(currentImage);
                                }
                            }
                        }
                        break;
                }
            };
        }

    }
}