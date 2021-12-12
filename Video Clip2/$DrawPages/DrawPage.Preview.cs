using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using Video_Clip2.Clips;
using Video_Clip2.Clips.ClipManagers;
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

                if (e.NewSize.Width != e.PreviousSize.Width)
                {
                    ScrollViewer element = this.AppBarRightScrollViewer;
                    int half = element.HorizontalAlignment == HorizontalAlignment.Right ? 2 : 1;
                    element.MaxWidth = e.NewSize.Width / half - this.PlayGrid.Width - element.Margin.Right;
                }

                if (e.NewSize.Height != e.PreviousSize.Height)
                {
                    double height = e.NewSize.Height;
                    this.PreviewGrid.SetWindowHeight(height - this.PlayGrid.Width);
                }
            };
            this.FullScreenButton.Click += (s, e) =>
            {
                double height = base.ActualHeight;
                this.Animation.To = this.PreviewGrid.FullScreenStarted(height - 48);
                this.Storyboard.Begin(); // Storyboard
            };
            this.Storyboard.Completed += (s, e) => this.PreviewGrid.FullScreenCompleted();



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
                switch (GroupIndex)
                {
                    case 6:
                        this.DurationMenu.Draw(args.DrawingSession, s.Size);
                        break;

                    default:
                        for (int i = 0; i < rows; i++)
                        {
                            foreach (IClip item in this.ViewModel.ObservableCollection)
                            {
                                if (item.Visibility == Visibility.Collapsed) continue;
                                if (item.Opacity == 0) continue;
                                if (item.Index != i) continue;

                                ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, s.Size);
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
                        break;
                }
            };
        }

    }
}