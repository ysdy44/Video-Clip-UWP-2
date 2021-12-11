using System;
using Video_Clip2.Clips;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructMenu()
        {
            this.BackButton.Click += (s, e) =>  this.GroupIndex = 0;
            this.SpeedButton.Click += (s, e) =>  this.GroupIndex = 0;


            this.ExportButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = this.GroupIndex == 0 ? 1 : 0;
            };


            this.PropertyButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = this.GroupIndex == 0 ? 2 : 0;
            };


            this.TransitionButton.Click += (s, e) =>
            {
                this.GroupIndex = this.GroupIndex == 0 ? 3 : 0;
            };


            this.EasingButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = this.GroupIndex == 0 ? 4 : 0;
            };


            this.EffectButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = this.GroupIndex == 0 ? 5 : 0;
            };


            this.DurationButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = this.GroupIndex == 0 ? 6 : 0;
            };
        }

    }
}