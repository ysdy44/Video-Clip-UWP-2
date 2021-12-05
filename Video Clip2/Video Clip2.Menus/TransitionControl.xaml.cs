using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class TransitionMenu : UserControl
    {

        //@Converter
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        public TransitionMenu()
        {
            this.InitializeComponent();
        }

    }
}