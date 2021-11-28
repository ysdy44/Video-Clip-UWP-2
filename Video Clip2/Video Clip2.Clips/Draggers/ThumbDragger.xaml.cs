using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Draggers
{
    public sealed partial class ThumbDragger : DraggerBase
    {

        public ThumbDragger()
        {
            this.InitializeComponent();
        }

        public override void Update(ListViewSelectionMode selectionMode, Trimmer trimmer, double trackHeight)
        {
            switch (selectionMode)
            {
                case ListViewSelectionMode.None:
                    base.Visibility = Visibility.Collapsed;
                    break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    Rect rect = trimmer.ToRect(trackHeight);
                    Canvas.SetLeft(this, rect.Left);
                    Canvas.SetTop(this, rect.Top);
                    base.Width = rect.Width;
                    base.Height = rect.Height;
                    base.Visibility = Visibility.Visible;
                    break;
            }
        }

    }
}