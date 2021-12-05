using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class EffectMenu : UserControl
    {

        //@Converter
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        public EffectMenu()
        {
            this.InitializeComponent();
        }

    }
}