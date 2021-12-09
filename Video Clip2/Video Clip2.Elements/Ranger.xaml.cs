using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Video_Clip2.Elements
{
    public sealed partial class Ranger : UserControl
    {

        readonly double ThumbWidth = 22;
        readonly double ThumbHeight = 50;

        TimeSpan MinDuration = TimeSpan.FromSeconds(2);
        TimeSpan OriginalDuration = TimeSpan.FromSeconds(10);
        double TrimmedDuration => 1 - this.TrimTimeFromStart - this.TrimTimeFromEnd;
        double TrimTimeFromStart = 0;
        double TrimTimeFromEnd = 0;

        double StartingMinDuration; // Percentage
        double StartingOriginalDuration; // Length
        double StartingTrimTimeFromStart; // Percentage
        double StartingTrimTimeFromEnd; // Percentage

        public Ranger()
        {
            this.InitializeComponent();

            this.Canvas.Margin = new Thickness(this.ThumbWidth, 0, this.ThumbWidth, 0);
            this.StartThumb.Width = this.ThumbWidth;
            this.EndThumb.Width = this.ThumbWidth;
            this.TrackRect.Height = this.ThumbHeight;
            this.DecreaseRect.Height = this.ThumbHeight;
            this.StartThumb.Height = this.ThumbHeight;
            this.EndThumb.Height = this.ThumbHeight;

            base.Loaded += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                this.UpdateWidth(width);
            };
            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                double width = e.NewSize.Width - this.ThumbWidth - this.ThumbWidth;
                double height = e.NewSize.Height;
                this.Canvas.Width = width;
                this.TrackRect.Width = width;
                this.RightLine.X1 = width;
                this.RightLine.X2 = width;
                this.StartLine.Y2 = height;
                this.EndLine.Y2 = height;

                double top = (height - this.ThumbHeight) / 2;
                Canvas.SetTop(this.StartThumb, top);
                Canvas.SetTop(this.EndThumb, top);
                Canvas.SetTop(this.TrackRect, top);
                Canvas.SetTop(this.DecreaseRect, top);
                Canvas.SetTop(this.StartThumb, top);
                Canvas.SetTop(this.EndThumb, top);
                this.LeftLine.Y1 = top;
                this.RightLine.Y1 = top;
                this.LeftLine.Y2 = top + this.ThumbHeight;
                this.RightLine.Y2 = top + this.ThumbHeight;

                this.UpdateWidth(width);
            };


            this.DecreaseRect.PointerReleased += this.ThumbPointerReleased;
            this.DecreaseRect.PointerPressed += this.ThumbPointerPressed;
            this.DecreaseRect.ManipulationStarted += this.ThumbManipulationStarted;
            this.DecreaseRect.ManipulationDelta += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                double cumulative = Math.Max(-this.StartingTrimTimeFromStart, Math.Min(this.StartingTrimTimeFromEnd, e.Cumulative.Translation.X / width));
                this.TrimTimeFromStart = this.StartingTrimTimeFromStart + cumulative;
                this.TrimTimeFromEnd = this.StartingTrimTimeFromEnd - cumulative;
                this.UpdateWidth(width);
                e.Handled = true;
            };
            this.DecreaseRect.ManipulationCompleted += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                double cumulative = Math.Max(-this.StartingTrimTimeFromStart, Math.Min(this.StartingTrimTimeFromEnd, e.Cumulative.Translation.X / width));
                this.TrimTimeFromStart = this.StartingTrimTimeFromStart + cumulative;
                this.TrimTimeFromEnd = this.StartingTrimTimeFromEnd - cumulative;
                e.Handled = true;
            };


            this.StartThumb.PointerReleased += this.ThumbPointerReleased;
            this.StartThumb.PointerPressed += this.ThumbPointerPressed;
            this.StartThumb.ManipulationStarted += this.ThumbManipulationStarted;
            this.StartThumb.ManipulationDelta += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                this.TrimTimeFromStart = this.GetTrimTimeFromStart(e.Cumulative.Translation.X / width);
                this.UpdateWidth(width);
                e.Handled = true;
            };
            this.StartThumb.ManipulationCompleted += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                this.TrimTimeFromStart = this.GetTrimTimeFromStart(e.Cumulative.Translation.X / width);
                e.Handled = true;
            };


            this.EndThumb.PointerReleased += this.ThumbPointerReleased;
            this.EndThumb.PointerPressed += this.ThumbPointerPressed;
            this.EndThumb.ManipulationStarted += this.ThumbManipulationStarted;
            this.EndThumb.ManipulationDelta += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                this.TrimTimeFromEnd = this.GetTrimTimeFromEnd(e.Cumulative.Translation.X / width);
                this.UpdateWidth(width);
                e.Handled = true;
            };
            this.EndThumb.ManipulationCompleted += (s, e) =>
            {
                double width = this.Canvas.ActualWidth;
                this.TrimTimeFromEnd = this.GetTrimTimeFromEnd(e.Cumulative.Translation.X / width);
                e.Handled = true;
            };
        }


        public void GetDuration(out TimeSpan trimTimeFromStart, out TimeSpan trimTimeFromEnd)
        {
            double width = this.OriginalDuration.ToDouble();
            trimTimeFromStart = (width * this.TrimTimeFromStart).ToTimeSpan();
            trimTimeFromEnd = (width * this.TrimTimeFromEnd).ToTimeSpan();
        }
        public void SetDuration(TimeSpan originalDuration, TimeSpan minDuration, TimeSpan trimTimeFromStart, TimeSpan trimTimeFromEnd)
        {
            this.OriginalDuration = originalDuration;
            this.MinDuration = minDuration;
            this.TrimTimeFromStart = trimTimeFromStart.ToDouble() / originalDuration.ToDouble();
            this.TrimTimeFromEnd = trimTimeFromEnd.ToDouble() / originalDuration.ToDouble();
         
            double width = this.Canvas.ActualWidth;
            this.UpdateWidth(width);
        }


        private void ThumbPointerReleased(object sender, PointerRoutedEventArgs e) => base.ReleasePointerCapture(e.Pointer);
        private void ThumbPointerPressed(object sender, PointerRoutedEventArgs e) => base.CapturePointer(e.Pointer);
        private void ThumbManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.StartingMinDuration = this.MinDuration.ToDouble() / this.OriginalDuration.ToDouble();
            this.StartingOriginalDuration = this.OriginalDuration.ToDouble();
            this.StartingTrimTimeFromStart = this.TrimTimeFromStart;
            this.StartingTrimTimeFromEnd = this.TrimTimeFromEnd;
            e.Handled = true;
        }


        private double GetTrimTimeFromStart(double cumulative) => Math.Max(0, Math.Min(1 - this.StartingTrimTimeFromEnd - this.StartingMinDuration, this.StartingTrimTimeFromStart + cumulative));
        private double GetTrimTimeFromEnd(double cumulative) => Math.Max(0, Math.Min(1 - this.StartingTrimTimeFromStart - this.StartingMinDuration, this.StartingTrimTimeFromEnd - cumulative));
        private void UpdateWidth(double width)
        {
            // Start
            {
                double start = width * this.TrimTimeFromStart;
                double left = start;
                Canvas.SetLeft(this.DecreaseRect, left);
                Canvas.SetLeft(this.StartThumb, left - this.ThumbWidth);
                this.StartLine.X1 = left;
                this.StartLine.X2 = left;

                this.StartTextBlock.Text = TimeSpan.FromSeconds(this.StartingOriginalDuration * this.TrimTimeFromStart).ToText();
                double textWidth = this.StartTextBlock.ActualWidth;
                if (left < textWidth)
                    this.StartTextBlock.Visibility = Visibility.Collapsed;
                else
                {
                    double textLeft = (left - textWidth) / 2;
                    Canvas.SetLeft(this.StartTextBlock, textLeft);
                    this.StartTextBlock.Visibility = Visibility.Visible;
                }
            }

            // End
            {
                double end = width * this.TrimTimeFromEnd;
                double left = width - end;
                Canvas.SetLeft(this.EndThumb, left);
                this.EndLine.X1 = left;
                this.EndLine.X2 = left;

                this.EndTextBlock.Text = TimeSpan.FromSeconds(this.StartingOriginalDuration * this.TrimTimeFromEnd).ToText();
                double textWidth = this.EndTextBlock.ActualWidth;
                if (end < textWidth)
                    this.EndTextBlock.Visibility = Visibility.Collapsed;
                else
                {
                    double textLeft = width - (end + textWidth) / 2;
                    Canvas.SetLeft(this.EndTextBlock, textLeft);
                    this.EndTextBlock.Visibility = Visibility.Visible;
                }
            }

            // Duration
            {
                double duration = width * this.TrimmedDuration;
                this.DecreaseRect.Width = duration;

                this.DurationTextBlock.Text = TimeSpan.FromSeconds(this.StartingOriginalDuration * this.TrimmedDuration).ToText();
                double textWidth = this.DurationTextBlock.ActualWidth;
                if (duration < textWidth)
                    this.DurationTextBlock.Visibility = Visibility.Collapsed;
                else
                {
                    double start = Canvas.GetLeft(this.DecreaseRect);
                    double textLeft = start + (duration - textWidth) / 2;
                    Canvas.SetLeft(this.DurationTextBlock, textLeft);
                    this.DurationTextBlock.Visibility = Visibility.Visible;
                }
            }
        }
    }
}