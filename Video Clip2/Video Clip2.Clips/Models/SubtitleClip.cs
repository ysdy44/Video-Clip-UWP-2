using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using Video_Clip2.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public struct Subtitle
    {
        public string Text;
        public TimeSpan Delay;
        public TimeSpan Duration;
    }

    public class SubtitleClip : TextClip, IClip
    {

        public IList<Subtitle> Subtitles { get; private set; }

        public override ClipType Type => ClipType.Subtitle;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.OrangeRed, Symbol.FontColor);

        public SubtitleClip(IList<Subtitle> subtitles, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale, ICanvasResourceCreatorWithDpi resourceCreator)
           : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.CommandList = new CanvasCommandList(resourceCreator);
            base.Text = SubtitleClip.GetIndex(subtitles, position, delay);
            if (base.Text != null) TextClip.Render(this.CommandList, base.Text);
            base.ChangeView(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            return new SubtitleClip(this.Subtitles, isMuted, position, this.Delay, nextDuration, base.Index, trackHeight, trackScale, this.ResourceCreator);
        }

        //@Static
        private static string GetIndex(IList<Subtitle> subtitles, TimeSpan position, TimeSpan delay)
        {
            TimeSpan p = position - delay;
            foreach (Subtitle item in subtitles)
            {
                if (p >= item.Delay)
                {
                    if (p < item.Delay + item.Duration)
                    {
                        return item.Text;
                    }
                }
            }
            return null;
        }

    }
}