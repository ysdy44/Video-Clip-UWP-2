using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class OpacityMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private string DoubleToStringConverter(double value) => ((int)value).ToString();
        private TextDecorations DoubleToUnderlineStyleConverter(double value) => value == 100 ? TextDecorations.None : TextDecorations.Underline;

        //@Content
        public string Title { get => this.TitleRun.Text; set => this.TitleRun.Text = value; }

        public OpacityMenu()
        {
            this.InitializeComponent();
            this.Slider.ValueChangedStarted += (s, e) =>
            {
                //this.ViewModel.IsPlayingCore = false;
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
                float opacity = (float)(e.NewValue / 100);

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        clip.Opacity = opacity;
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.Slider.ValueChangedCompleted += (s, e) =>
            {
                //this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };
            this.Slider.ValueChangedUnfocused += (s, e) =>
            {
                float opacity = (float)(e.NewValue / 100);

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        clip.Opacity = opacity;
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.Button.Click += (s, e) =>
            {
                if (this.Slider.Value == 100) return;

                this.Slider.Value = 100;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        clip.Opacity = 1;
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
                    this.Slider.Value = clip.Opacity * 100;
                    break;
                }
            }
        }

    }
}