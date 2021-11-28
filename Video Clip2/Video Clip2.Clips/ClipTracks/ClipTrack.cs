using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.ClipTracks
{
    public sealed partial class ClipTrack : ClipTrackBase, IClipTrack
    {
        public ClipTrack(Color color, Symbol symbol)
            : base(color, symbol)
        {
            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                base.SetSize(e.NewSize.Width, e.NewSize.Height);
            };
        }
    }
}