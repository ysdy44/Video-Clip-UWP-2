using System;
using Video_Clip2.Clips;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        double StartingPositionX = 0;

        private void ConstructPosition()
        {
            this.PinButton.Click += (s, e) => Controls.PinCanvas.Pin(this.ViewModel.Position, this.ViewModel.PinCollection);

            this.ScaleSlider.ValueChangedStarted += (s, e) => this.ViewModel.IsPlayingCore = false;
            this.ScaleSlider.ValueChangedCompleted += (s, e) => this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;

            this.PositionCanvas.PointerReleased += (s, e) => base.ReleasePointerCapture(e.Pointer);
            this.PositionCanvas.PointerPressed += (s, e) => base.CapturePointer(e.Pointer);
            this.PositionCanvas.ManipulationStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
                this.StartingPositionX = e.Position.X;
                this.ViewModel.Position = e.Position.X.ToTimeSpan(this.ViewModel.TrackScale);
                e.Handled = true;
            };
            this.PositionCanvas.ManipulationDelta += (s, e) =>
            {
                double x = e.Cumulative.Translation.X + this.StartingPositionX;
                TimeSpan position = x.ToTimeSpan(this.ViewModel.TrackScale);
                this.ViewModel.Position = this.ViewModel.GetPosition(position);
                e.Handled = true;
            };
            this.PositionCanvas.ManipulationCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
                e.Handled = true;
            };
        }

    }
}