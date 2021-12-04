using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Linq;
using Video_Clip2.Clips;
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
        public void Init()
        {
            IClip clip = this.ViewModel.ObservableCollection.FirstOrDefault();
            if (clip == null) return;

            IClip next = this.Next(clip);
            if (next == null) return;

            Init(clip, next, this.ViewModel.TrackScale, this.ViewModel.TrackHeight);
        }
        public void Init(IClip left, IClip right, double trackScale, double trackHeight)
        {
            int index = left.Index;
            double leftX = (left.Delay + left.Duration).ToDouble(trackScale);
            double rightX = right.Delay.ToDouble(trackScale);
            double duration = 4 * trackScale;

            //     Canvas.SetLeft(AAAAAAAAAAAAAA, leftX - duration);
            //     Canvas.SetTop(AAAAAAAAAAAAAA, index * trackHeight);
            //     AAAAAAAAAAAAAA.Width = duration;
            //     AAAAAAAAAAAAAA.Height = trackHeight;

            double rs = leftX - duration - rightX;
            right.CacheDelay(trackScale);
            right.AddDelay(trackScale, rs, TimeSpan.Zero);
        }

        private IClip Last(IClip clip)
        {
            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    if (item.Index == clip.Index)
                    {
                        if (item.Id != clip.Id)
                        {
                            if (item.Delay + item.Duration <= clip.Delay)
                            {
                                return item;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private IClip Next(IClip clip)
        {
            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    if (item.Index == clip.Index)
                    {
                        if (item.Id != clip.Id)
                        {
                            if (item.Delay >= clip.Delay + clip.Duration)
                            {
                                return item;
                            }
                        }
                    }
                }
            }

            return null;
        }

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

                for (int i = 0; i < rows; i++)
                {
                    foreach (IClip item in this.ViewModel.ObservableCollection)
                    {
                        if (item.Visibility == Visibility.Visible)
                        {
                            if (item.Index == i)
                            {
                                double value = 0;
                                TransitionVisibility visibility = item.Transition.GetVisibility(this.ViewModel.Position, item.Delay, item.Delay + item.Duration, ref value);
                                switch (visibility)
                                {
                                    case TransitionVisibility.Collapsed:
                                        break;
                                    case TransitionVisibility.Visible:
                                        {
                                            ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, this.PreviewCanvas, s.Size);
                                            args.DrawingSession.DrawImage(Effect.Render(item.Effect, image, scale));
                                        }
                                        break;
                                    default:
                                        {
                                            ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, this.PreviewCanvas, s.Size);
                                            args.DrawingSession.DrawImage(new OpacityEffect
                                            {
                                                Source = Effect.Render(item.Effect, image, scale),
                                                Opacity = (float)value
                                            });
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            };
        }

    }
}