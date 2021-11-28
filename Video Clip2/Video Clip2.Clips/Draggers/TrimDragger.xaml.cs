using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Draggers
{
    public abstract partial class TrimDragger : DraggerBase
    {

        //@Abstract
        public abstract CornerRadius Radius { get; }
        public abstract double GetValue(Trimmer trimmer);

        bool IsManipulationStarted;

        public TrimDragger()
        {
            this.InitializeComponent();
            this.Border.CornerRadius = this.Radius;

            base.PointerExited += (s, e) =>
            {
                if (this.IsManipulationStarted) return;

                // Cursor
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
                VisualStateManager.GoToState(this, this.Normal.Name, false); // VisualState
            };
            base.PointerEntered += (s, e) =>
            {
                if (this.IsManipulationStarted) return;

                // Cursor
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeWestEast, 0);
                VisualStateManager.GoToState(this, this.PointerOver.Name, false); // VisualState
            };
        }

        public void UpdateManipulationStarted()
        {
            this.IsManipulationStarted = true;

            // Cursor
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeWestEast, 0);
            VisualStateManager.GoToState(this, this.Pressed.Name, false); // VisualState
        }
        public void UpdateManipulationCompleted()
        {
            this.IsManipulationStarted = false;

            // Cursor
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            VisualStateManager.GoToState(this, this.Normal.Name, false); // VisualState
        }


        public override void Update(ListViewSelectionMode selectionMode, Trimmer trimmer, double trackHeight)
        {
            switch (selectionMode)
            {
                case ListViewSelectionMode.None:
                case ListViewSelectionMode.Multiple:
                    base.Visibility = Visibility.Collapsed;
                    break;
                case ListViewSelectionMode.Single:
                    Canvas.SetLeft(this, this.GetValue(trimmer));
                    Canvas.SetTop(this, trackHeight * (trimmer.Top + trimmer.Bottom + 1) / 2 - trackHeight / 2);
                    base.Visibility = Visibility.Visible;
                    break;
            }
        }

    }
}