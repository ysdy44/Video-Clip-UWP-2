using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class OpacityMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public OpacityMenu()
        {
            this.InitializeComponent();
            this.Slider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
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
                    this.Slider.Value = clip.Opacity * 100;
                    break;
                }
            }
        }

        public void OnNavigatedTo()
        {
        }
    }
}