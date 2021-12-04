using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructMenu()
        {
            this.ExportBackButton.Click += (s, e) => this.GroupIndex = 0;
            this.ExportButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 1;

                this.ExportOKButton.Focus(FocusState.Keyboard);
            };


            this.PropertyBackButton.Click += (s, e) => this.GroupIndex = 0;
            this.PropertyButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 2;

                IClip clip = this.ViewModel.ObservableCollection.FirstOrDefault(c => c.IsSelected);
                if (clip == null) return;
                this.PropertyPivot.SelectedIndex = (int)clip.Type - 1;
            };


            this.TransitionBackButton.Click += (s, e) => this.GroupIndex = 0;
            this.TransitionButton.Click += (s, e) =>
            {
                this.GroupIndex = 3;
            };


            this.EasingBackButton.Click += (s, e) => this.GroupIndex = 0;
            this.EasingButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 4;
            };


            this.EffectBackButton.Click += (s, e) => this.GroupIndex = 0;
            this.EffectButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 5;
            };
        }

    }
}