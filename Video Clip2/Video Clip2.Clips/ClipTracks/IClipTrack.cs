using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.ClipTracks
{
    public interface IClipTrack
    {
        Button Self { get; }

        event TypedEventHandler<CanvasControl, CanvasDrawEventArgs> Draw;
        event RoutedEventHandler TrackUnloaded;
        event RoutedEventHandler TrackLoaded;

        void Invalidate();

        void SetLeft(double trackScale, TimeSpan delay);
        void SetTop(int index, double trackHeight);
        void SetWidth(double trackScale, TimeSpan duration);
        void SetHeight(double trackHeight);
        void SetisSelected(bool isSelected);

        void SetIsMuted(bool isMuted, bool isMutedCore);

        bool Contains(Rect rect);

    }
}