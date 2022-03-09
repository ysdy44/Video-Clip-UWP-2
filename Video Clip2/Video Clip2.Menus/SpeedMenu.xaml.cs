using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class SpeedMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private string DoubleToStringConverter(double value) => (value / 4).ToString("F2");
        private TextDecorations DoubleToUnderlineStyleConverter(double value) => value == 4 ? TextDecorations.None : TextDecorations.Underline;

        //@Content
        public string Title { get => this.TitleRun.Text; set => this.TitleRun.Text = value; }

        private double OriginalDuration;

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
                double speed = e.NewValue / 4;
                double originalDuration = this.OriginalDuration;
                this.SpeedDurationRun.Text = (originalDuration / speed).ToTimeSpan().ToText();

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
            this.Slider.ValueChangedUnfocused += (s, e) =>
            {
                double speed = e.NewValue / 4;
                double originalDuration = this.OriginalDuration;
                this.SpeedDurationRun.Text = (originalDuration / speed).ToTimeSpan().ToText();

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
                                    mediaClip.ResetPlaybackRate(speed, this.ViewModel.TrackScale);
                                }
                                break;
                        }
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.Button.Click += (s, e) =>
            {
                if (this.Slider.Value == 4) return;

                double originalDuration = this.OriginalDuration;
                this.SpeedDurationRun.Text = originalDuration.ToTimeSpan().ToText();
                this.Slider.Value = 4;

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
                                    mediaClip.ResetPlaybackRate(1, this.ViewModel.TrackScale);
                                }
                                break;
                        }
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
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
                                double speed = mediaClip.PlaybackRate;
                                double originalDuration = mediaClip.OriginalDuration.ToDouble();
                                string originalDuration2 = mediaClip.OriginalDuration.ToText();

                                this.OriginalDuration = originalDuration;
                                this.Slider.Value = speed * 4;
                                this.SpeedDurationRun.Text = (originalDuration / speed).ToTimeSpan().ToText();
                                this.OriginalDurationRun.Text = originalDuration2;
                                break;
                            }
                            break;
                    }
                }
            }
        }

    }
}