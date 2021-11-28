using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Elements
{
    public sealed partial class ThemeControl : UserControl
    {
        public ElementTheme Theme
        {
            set
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.RequestedTheme == value) return;
                    frameworkElement.RequestedTheme = value;
                }
            }
        }

        public ThemeControl()
        {
            this.InitializeComponent();

            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                ElementTheme theme = frameworkElement.RequestedTheme;
                this.DefaultButton.IsChecked = theme == ElementTheme.Default;
                this.LightButton.IsChecked = theme == ElementTheme.Light;
                this.DarkButton.IsChecked = theme == ElementTheme.Dark;
            }

            this.DefaultButton.Click += (s, e) => this.Theme = ElementTheme.Default;
            this.LightButton.Click += (s, e) => this.Theme = ElementTheme.Light;
            this.DarkButton.Click += (s, e) => this.Theme = ElementTheme.Dark;
        }
    }
}
