using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Video_Clip2.Tools.Models
{
    public class NoneTool : ITool
    {

        public ManipulationModes TrackManipulationMode => ManipulationModes.System;

        public void TrackManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { }
        public void TrackManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) { }
        public void TrackManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }


        public void ItemClick(object sender, RoutedEventArgs e) { }

        public void ItemManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { }
        public void ItemManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) { }
        public void ItemManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }


        public void ThumbDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { }
        public void ThumbDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) { }
        public void ThumbDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }

        public void LeftDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { }
        public void LeftDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) { }
        public void LeftDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }

        public void RightDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { }
        public void RightDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) { }
        public void RightDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }

    }
}