using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Video_Clip2.Tools
{
    public interface ITool
    {

        ManipulationModes TrackManipulationMode { get; }

        void TrackManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e);
        void TrackManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);
        void TrackManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);


        void ItemClick(object sender, RoutedEventArgs e);

        void ItemManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e);
        void ItemManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);
        void ItemManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);


        void ThumbDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e);
        void ThumbDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);
        void ThumbDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);

        void LeftDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e);
        void LeftDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);
        void LeftDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);

        void RightDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e);
        void RightDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);
        void RightDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);

    }
}