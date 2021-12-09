using System;
using Video_Clip2.Clips;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructMenu()
        {
            this.BackButton.Click += (s, e) =>
            {
                switch (this.GroupIndex)
                {
                    case 6:
                        {
                            this.DurationRanger.GetDuration(out TimeSpan trimTimeFromStart, out TimeSpan trimTimeFromEnd);

                            foreach (IClip item in this.ViewModel.ObservableCollection)
                            {
                                if (item.IsSelected)
                                {
                                    switch (item.Type)
                                    {
                                        case ClipType.Video:
                                        case ClipType.Audio:
                                            if (item is MediaClip mediaClip)
                                            {
                                                mediaClip.SetDuration(this.ViewModel.TrackScale, trimTimeFromStart, trimTimeFromEnd);
                                                break;
                                            }
                                            break;
                                    }
                                }
                            }

                            this.SelectionViewModel.SetMode(); // Selection
                            this.ViewModel.Invalidate(); // Invalidate
                        }
                        break;
                    default:
                        break;
                }
                this.GroupIndex = 0;
            };


            this.ExportButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 1;
            };


            this.PropertyButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 2;
            };


            this.TransitionButton.Click += (s, e) =>
            {
                this.GroupIndex = 3;
            };


            this.EasingButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 4;
            };


            this.EffectButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 5;
            };
        }

    }
}