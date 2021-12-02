using System.Linq;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.Models;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructEdit()
        {
            this.EditEditItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

                IClip clip = this.ViewModel.ObservableCollection.FirstOrDefault(c => c.IsSelected);
                if (clip == null) return;

                this.GroupIndex = 2;
                this.EditPivot.SelectedIndex = (int)clip.Type - 1;
            };


            this.EditEasingItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

                this.GroupIndex = 4;
            };


            this.EditEffectItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

                IClip clip = this.ViewModel.ObservableCollection.FirstOrDefault(c => c.IsSelected);
                if (clip == null) return;

                this.GroupIndex = 5;
            };


            this.EditCutItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

            };


            this.EditPasteItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

            };


            this.EditDeleteItem.Click += (s, e) =>
            {
                this.EditFlyout.Hide();
                this.ViewModel.IsPlaying = false;

                this.ViewModel.MethodEditClear();
            };
        }

    }
}