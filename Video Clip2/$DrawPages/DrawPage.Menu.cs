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
                this.GroupIndex = 1;
                this.ExportOKButton.Focus(FocusState.Keyboard);
            };


            this.EditBackButton.Click += (s, e) => this.GroupIndex = 0;
    


            this.EasingBackButton.Click += (s, e) => this.GroupIndex = 0;


            this.EffectBackButton.Click += (s, e) => this.GroupIndex = 0;
        }

    }
}