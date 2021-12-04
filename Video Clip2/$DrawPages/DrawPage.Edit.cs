using System.Linq;
using Video_Clip2.Clips.Clips;
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