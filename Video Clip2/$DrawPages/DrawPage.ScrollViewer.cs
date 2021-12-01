using Video_Clip2.Clips;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        bool IsDirectSliderScale;
        bool IsDirectTouchScale;
        bool IsDirectWheelMove;
        bool IsDirectTouchMove;

        private void ConstructScrollViewer()
        {
            // Wheel Move
            // Wheel Scale
            // Touch Move
            // Touch Scale
            // Slider Scale
            this.PinHeader.XChanged += (s, value) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;
                if (this.IsDirectWheelMove) return;
                if (this.IsDirectTouchMove) return;

                double horizontalOffset = value;
                bool disableAnimation = value != 0;
                this.TrackScrollViewer.ChangeView(horizontalOffset, null, null, disableAnimation);
            };


            this.TrackScrollViewer.SizeChanged += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;

                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.UpdateTrackWidth(e.NewSize.Width);
                this.UpdateTrackX();
            };

            // Wheel Move
            // Wheel Scale
            // Touch Move
            // Touch Scale
            // Slider Scale
            this.TrackScrollViewer.ViewChanging += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;
                if (this.IsDirectTouchMove) return;

                this.IsDirectWheelMove = true;

                this.UpdateTrackPosition();
            };
            // Wheel Move
            // Wheel Scale
            // Touch Move
            // Touch Scale
            // Slider Scale
            this.TrackScrollViewer.ViewChanged += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;

                if (this.IsDirectTouchMove)
                {
                    this.UpdateTrackPosition();
                }
                else
                {
                    this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                    this.IsDirectWheelMove = false;

                    this.UpdateTrackPosition();
                }
            };

            // Touch Move
            this.TrackScrollViewer.DirectManipulationStarted += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;

                this.IsDirectTouchMove = true;
                this.ViewModel.IsPlayingCore = false;
            };
            // Touch Move
            // Touch Scale
            this.TrackScrollViewer.DirectManipulationCompleted += (s, e) =>
            {
                if (this.IsDirectFreedom) return;
                if (this.IsDirectSliderScale) return;
                if (this.IsDirectTouchScale) return;

                if (this.IsDirectTouchMove)
                {
                    this.IsDirectTouchMove = false;
                    this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                }
            };


            // Touch Scale
            this.TrackCanvas.ManipulationStarted += (s, e) =>
            {
                this.IsDirectTouchScale = this.TrackCanvas.ManipulationMode.HasFlag(ManipulationModes.Scale);
            };
            // Touch Scale
            this.TrackCanvas.ManipulationDelta += (s, e) =>
            {
                if (this.IsDirectTouchScale)
                {
                    this.UpdateTrackX();
                }
            };
            // Touch Scale
            this.TrackCanvas.ManipulationCompleted += (s, e) =>
            {
                this.IsDirectTouchScale = false;
            };


            // Slider Scale
            this.ScaleSlider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
                this.IsDirectSliderScale = true;
            };
            // Slider Scale
            this.ScaleSlider.ValueChangedDelta += (s, e) =>
            {
                if (this.IsDirectFreedom) return;

                this.UpdateTrackX();
            };
            // Slider Scale
            this.ScaleSlider.ValueChangedCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                this.IsDirectSliderScale = false;
            };
        }

        private void UpdateTrackWidth(double width)
        {
            double widthHalf = width / 2;
            double trackLeftWidth = this.PinHeader.TrackLeftWidth;
            this.TrackCanvas.Padding = new Thickness(widthHalf - trackLeftWidth, 0, widthHalf + trackLeftWidth, 0);
        }
        private void UpdateTrackX()
        {
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