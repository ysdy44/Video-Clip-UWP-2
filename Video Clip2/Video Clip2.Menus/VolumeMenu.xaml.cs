using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class VolumeMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        public VolumeMenu()
        {
            this.InitializeComponent();
            this.Slider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
            };
            this.Slider.ValueChangedDelta += (s, e) =>
            {
                double volume = e.NewValue / 100;
                bool isMuted = e.NewValue == 0;
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
                                    mediaClip.SetVolume(volume);
                                    mediaClip.SetIsMuted(this.ViewModel.IsMuted, isMuted);
                                }
                                break;
                        }
                    }
                }
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
                                this.Slider.Value = mediaClip.Volume * 100;
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