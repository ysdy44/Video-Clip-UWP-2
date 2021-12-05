using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips;
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
            this.BackButton.Click += (s, e) => this.GroupIndex = 0;


            this.ExportButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 1;
            };


            this.PropertyButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 2;
            };


            this.TransitionButton.Click += (s, e) =>
            {
                this.GroupIndex = 3;
            };


            this.EasingButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 4;
            };


            this.EffectButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 5;
            };
        }

    }
}