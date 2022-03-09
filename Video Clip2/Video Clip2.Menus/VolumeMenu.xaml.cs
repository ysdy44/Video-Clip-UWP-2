using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class VolumeMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private string DoubleToStringConverter(double value) => ((int)value).ToString();
        private TextDecorations DoubleToUnderlineStyleConverter(double value) => value == 100 ? TextDecorations.None : TextDecorations.Underline;
        private string DoubleToIconStyleConverter(double value)
        {
            if (value == 0) return "\uE198";
            else if (value <= 25) return "\uE992";
            else if (value <= 50) return "\uE993";
            else return "\uE15D";
        }

        //@Content
        public string Title { get => this.TitleRun.Text; set => this.TitleRun.Text = value; }

        public VolumeMenu()
        {
            this.InitializeComponent();
            this.Slider.ValueChangedStarted += (s, e) =>
            {
                //this.ViewModel.IsPlayingCore = false;
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
                //this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };
            this.Slider.ValueChangedUnfocused += (s, e) =>
            {
                double volume = e.NewValue / 100;
                bool isMuted = e.NewValue == 0;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

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
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.Button.Click += (s, e) =>
            {
                if (this.Slider.Value == 100) return;

                this.Slider.Value = 100;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    switch (clip.Type)
                    {
                        case ClipType.Video:
                        case ClipType.Audio:
                            if (clip is MediaClip mediaClip)
                            {
                                mediaClip.SetVolume(1);
                                mediaClip.SetIsMuted(this.ViewModel.IsMuted, false);
                            }
                            break;
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
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
                                return;
                            }
                            break;
                    }
                }
            }
        }

    }
}