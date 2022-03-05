using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class SpeedMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public SpeedMenu()
        {
            this.InitializeComponent();
            this.Slider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                clip.CacheDuration(this.ViewModel.TrackScale);
                                break;
                        }
                    }
                }
            };
            this.Slider.ValueChangedDelta += (s, e) =>
            {
                double speed = e.NewValue;
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (clip is MediaClip mediaClip)
                                {
                                    mediaClip.SetPlaybackRate(speed, this.ViewModel.TrackScale);
                                }
                                break;
                        }
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.Slider.ValueChangedCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };
        }

        public void OnNavigatedFrom()
        {
            foreach (Clipping item in this.ViewModel.ObservableCollection)
            {
                IClip clip = item.Self;

                if (clip.IsSelected)
                {
                    switch (clip.Type)
                    {
                        case ClipType.Video:
                        case ClipType.Audio:
                            if (clip is MediaClip mediaClip)
                            {
                                this.Slider.Value = mediaClip.PlaybackRate;
                                break;
                            }
                            break;
                    }
                }
            }
        }

        public void OnNavigatedTo()
        {
        }
    }
}