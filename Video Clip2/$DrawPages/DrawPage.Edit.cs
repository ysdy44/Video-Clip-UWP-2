using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Video_Clip2.Clips;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructEdit()
        {
            this.TrimButton.Click += (s, e) => this.ViewModel.MethodEditTrim();


            this.OpacityButton.Click += (s, e) =>
            {
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        this.OpacitySlider.Opacity = item.Opacity * 100;
                        break;
                    }
                }
                this.OpacitySlider.Width = this.AppBarRightStackPanel.ActualWidth;
                this.OpacityFlyout.ShowAt(this.AppBarRightStackPanel);
            };
            this.OpacitySlider.ValueChangedStarted += (s, e) =>
            {

            };
            this.OpacitySlider.ValueChangedDelta += (s, e) =>
            {
                float opacity = (float)(e.NewValue / 100);
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        item.Opacity = opacity;
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.OpacitySlider.ValueChangedCompleted += (s, e) =>
            {

            };


            this.ColorPicker.ColorChanged += (s, e) =>
            {
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        if (item is ColorClip colorClip)
                        {
                            colorClip.SetColor(e.NewColor);
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.EditCutItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditCopyItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditPasteItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditClearItem.Click += (s, e) => this.ViewModel.MethodEditClear();
        }

    }
}