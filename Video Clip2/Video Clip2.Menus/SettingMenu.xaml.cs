using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class SettingMenu : UserControl
    {

        //@Converter
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        public SettingMenu()
        {
            this.InitializeComponent();
        }

    }
}