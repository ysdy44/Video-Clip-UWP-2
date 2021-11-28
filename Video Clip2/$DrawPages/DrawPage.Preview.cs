using Microsoft.Graphics.Canvas;
using System;
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

        double StartingSpliterHeight;

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
                                ICanvasImage image = item.GetRender(this.ViewModel.IsPlayingCore, this.ViewModel.Position, this.PreviewCanvas, this.ViewModel.PreviewSize);

                                if (image != null)
                                {
                                    args.DrawingSession.DrawImage(Effect.Render(item.Effect, image, scale));
                                }
                            }
                        }
                    }
                }
            };

            this.PreviewGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.ViewModel.Scale = Math.Min(e.NewSize.Width / this.ViewModel.Size.Width, e.NewSize.Height / this.ViewModel.Size.Height);
            };

            this.GridSpliterButton.ManipulationStarted += (s, e) =>
            {
                double height = this.PreviewRowDefinition.ActualHeight;
                this.StartingSpliterHeight = height;

                this.PreviewGrid.Width = this.PreviewGrid.ActualWidth;
                this.PreviewGrid.Height = this.PreviewGrid.ActualHeight;
                this.PreviewGrid.HorizontalAlignment = HorizontalAlignment.Center;
                this.PreviewGrid.VerticalAlignment = VerticalAlignment.Top;
            };
            this.GridSpliterButton.ManipulationDelta += (s, e) =>
            {
                double spliterHeight = this.StartingSpliterHeight + e.Cumulative.Translation.Y;
                this.PreviewRowDefinition.Height = new GridLength(spliterHeight < 0 ? 0 : spliterHeight);

                double height = this.PreviewRowDefinition.ActualHeight;
                double scale = height / this.StartingSpliterHeight;
                this.PreviewScaleTransform.ScaleX = scale;
                this.PreviewScaleTransform.ScaleY = scale;
            };
            this.GridSpliterButton.ManipulationCompleted += (s, e) =>
            {
                double spliterHeight = this.StartingSpliterHeight + e.Cumulative.Translation.Y;
                this.PreviewRowDefinition.Height = new GridLength(spliterHeight);

                this.PreviewScaleTransform.ScaleX = 1;
                this.PreviewScaleTransform.ScaleY = 1;

                this.PreviewGrid.Width = double.NaN;
                this.PreviewGrid.Height = double.NaN;
                this.PreviewGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.PreviewGrid.VerticalAlignment = VerticalAlignment.Stretch;
            };
        }

    }
}