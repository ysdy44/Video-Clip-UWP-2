using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Elements;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.ClipTracks
{
    public abstract partial class ClipTrackBase : Button
    {

        public Button Self => this;

        //@Delegate
        /// <summary>
        /// This is where the magic happens! 
        /// Hook this event to issue your immediate mode 2D drawing calls.
        /// </summary>
        public event TypedEventHandler<CanvasControl, CanvasDrawEventArgs> Draw
        {
            remove => this.CanvasControl.Draw -= value;
            add => this.CanvasControl.Draw += value;
        }
        public event RoutedEventHandler TrackUnloaded
        {
            remove => base.Unloaded -= value;
            add => base.Unloaded += value;
        }
        public event RoutedEventHandler TrackLoaded
        {
            remove => base.Loaded -= value;
            add => base.Loaded += value;
        }

        public ClipTrackBase(Color color, Symbol symbol)
        {
            this.InitializeComponent();
            this.BackBrush.Color = color;
            this.SymbolIcon.Symbol = symbol;
            this.CanvasControl.UseSharedDevice = true;
            this.CanvasControl.CustomDevice = ClipManager.CanvasDevice;

            base.Unloaded += (s, e) => this.CanvasControl.RemoveFromVisualTree();
            base.Loaded += (s, e) =>
            {
                this.SetSize(base.ActualWidth, base.ActualHeight);
                this.Storyboard.Begin(); // Storyboard
            };
        }


        public void Invalidate() => this.CanvasControl.Invalidate();
        protected void SetSize(double width, double height)
        {
            this.CanvasControl.Width = width;
            this.CanvasControl.Height = height;
        }

        public void SetLeft(double trackScale, TimeSpan delay) => Canvas.SetLeft(this, delay.ToDouble(trackScale));
        public void SetTop(int index, double trackHeight) => Canvas.SetTop(this, index * trackHeight);
        public void SetWidth(double trackScale, TimeSpan duration)
        {
            this.Width = duration.ToDouble(trackScale);
            this.DurationTextBlock.Text = duration.ToText();
        }
        public void SetHeight(double trackHeight) => base.Height = trackHeight;
        public void SetisSelected(bool isSelected)
        {
            Canvas.SetZIndex(this, isSelected ? 1 : 0);
            base.IsEnabled = !isSelected;
        }

        public void SetIsMuted(bool isMuted, bool isMutedCore)
        {
            this.MuteIcon.Visibility = isMuted ? Visibility.Visible : Visibility.Collapsed;
            this.MuteIcon2.Visibility = isMutedCore ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool Contains(Rect rect)
        {
            double yCenter = Canvas.GetTop(this) + base.ActualHeight / 2;
            double x = Canvas.GetLeft(this);
            double w = base.ActualWidth;

            Point leftCenter = new Point(x, yCenter);
            Point rightCenter = new Point(x + w, yCenter);
            return rect.Contains(leftCenter) && rect.Contains(rightCenter);
        }

    }
}