using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.Transforms;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructEdit()
        {
            this.TrimButton.Click += (s, e) => this.ViewModel.MethodEditTrim();
            this.ColorPicker.ColorChanged += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        if (clip is ColorClip colorClip)
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