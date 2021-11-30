using Video_Clip2.Clips;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        bool IsDirectMouseWheel;
        bool IsDirectManipulationStarted;

        private void ConstructScrollViewer()
        {
            this.PinHeader.XChanged += (s, value) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectManipulationStarted) return;
                if (this.IsDirectMouseWheel) return;

                if (this.ViewModel.IsPlayingCore)
                {
                    double horizontalOffset = value;
                    bool disableAnimation = true;
                    this.TrackScrollViewer.ChangeView(horizontalOffset, null, null, disableAnimation);
                }
                else if (value == 0)
                {
                    double horizontalOffset = 0;
                    bool disableAnimation = false;
                    this.TrackScrollViewer.ChangeView(horizontalOffset, null, null, disableAnimation);
                }
            };
            this.TrackScrollViewer.SizeChanged += (s, e) =>
            {
                if (this.IsDirectFreedom) return;

                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.UpdateTrackWidth(e.NewSize.Width);
            };

            this.TrackScrollViewer.ViewChanging += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectManipulationStarted) return;

                this.IsDirectMouseWheel = true;

                this.UpdateTrackPosition();
            };
            this.TrackScrollViewer.ViewChanged += (s, e) =>
            {
                if (this.IsDirectFreedom) return;

                if (this.IsDirectManipulationStarted)
                {
                    this.UpdateTrackPosition();
                }
                else
                {
                    this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                    this.IsDirectMouseWheel = false;

                    this.UpdateTrackPosition();
                }
            };

            this.TrackScrollViewer.DirectManipulationStarted += (s, e) =>
            {
                if (this.IsDirectFreedom) return;

                this.IsDirectManipulationStarted = true;
                this.ViewModel.IsPlayingCore = false;
            };
            this.TrackScrollViewer.DirectManipulationCompleted += (s, e) =>
            {
                if (this.IsDirectFreedom) return;

                if (this.IsDirectManipulationStarted)
                {
                    this.IsDirectManipulationStarted = false;
                    this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                }
            };
        }

        private void UpdateTrackWidth(double width)
        {
            double widthHalf = width / 2;
            double trackLeftWidth = this.PinHeader.TrackLeftWidth;
            this.TrackCanvas.Margin = new Thickness(widthHalf - trackLeftWidth, 0, widthHalf + trackLeftWidth, 0);

            double horizontalOffset = this.PinHeader.X;
            bool disableAnimation = true;
            this.TrackScrollViewer.ChangeView(horizontalOffset, null, null, disableAnimation);
        }
        private void UpdateTrackPosition()
        {
            double horizontalOffset = this.TrackScrollViewer.HorizontalOffset;
            double scale = this.ViewModel.TrackScale;
            this.ViewModel.Position = horizontalOffset.ToTimeSpan(scale);
        }

    }
}