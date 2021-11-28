using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.ClipTracks
{
    public sealed partial class LazyClipTrack : ClipTrackBase, IClipTrack
    {

        readonly DispatcherTimer Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(10)
        };

        public LazyClipTrack(Color color, Symbol symbol)
            : base(color, symbol)
        {
            this.Timer.Tick += (s, e) =>
            {
                this.Timer.Stop();

                base.SetSize(base.ActualWidth, base.ActualHeight);
            };

            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.Timer.Stop();
                this.Timer.Start();
            };
        }

    }
}