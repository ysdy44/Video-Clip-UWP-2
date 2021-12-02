using Video_Clip2.Clips.Clips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructCanvas()
        {
            this.TrackCanvas.PointerReleased += (s, e) => base.ReleasePointerCapture(e.Pointer);
            this.TrackCanvas.PointerPressed += (s, e) => base.CapturePointer(e.Pointer);
            this.TrackCanvas.Tapped += (s, e) =>
            {
                if (this.IsWheelForTrackScale) return;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                this.SelectionViewModel.SetModeNone(); // Selection
                e.Handled = true;
            };
            this.TrackCanvas.PointerWheelChanged += (s, e) =>
            {
                if (this.IsWheelForTrackScale == false) return;

                float space = e.GetCurrentPoint(this.TrackCanvas).Properties.MouseWheelDelta;
                if (space > 0)
                    this.ViewModel.TrackScale *= 1.1;
                else
                    this.ViewModel.TrackScale /= 1.1;

                e.Handled = true;
            };

            this.ItemCanvas.ItemRightTapped += (s, e) => this.ShowEdit(s);
            this.ItemCanvas.ItemHolding += (s, e) => this.ShowEdit(s);
            this.ItemCanvas.ItemTapped += (s, e) => e.Handled = true;

            this.ThumbDragger.RightTapped += (s, e) => this.ShowEdit();
            this.ThumbDragger.Holding += (s, e) => this.ShowEdit();
            this.ThumbDragger.Tapped += (s, e) =>
            {
                this.ShowEdit();
                e.Handled = true;
            };
        }

        private void ShowEdit()
        {
            this.EditFlyout.ShowAt(this.ThumbDragger);
        }
        private void ShowEdit(object sender)
        {
            if (sender is FrameworkElement placementTarget)
            {
                if (placementTarget.DataContext is IClip clip)
                {
                    foreach (IClip item in this.ViewModel.ObservableCollection)
                    {
                        if (item.IsSelected) item.IsSelected = false;
                    }

                    clip.IsSelected = false;
                    this.SelectionViewModel.SetModeSingle(clip); // Selection

                    this.EditFlyout.ShowAt(placementTarget);
                }
            }
        }

    }
}