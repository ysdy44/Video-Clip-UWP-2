using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class SubtitleClip : TextClipBase, IClip
    {

        public IList<Subtitle> Subtitles { get; set; }

        public override ClipType Type => ClipType.Subtitle;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.OrangeRed, Symbol.FontColor);

        public void Initialize(bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
        {
            base.Text = SubtitleClip.GetIndex(this.Subtitles, position, delay);
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeFrameClip(duration, trackScale);
            this.InitializeTextClipBase(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (this.Text == null) return null;
            else if (base.InRange(position) == false) return null;
            else return this.CommandList;
        }

        protected override IClip TrimClone(Clipping clipping, bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            // Clip
            SubtitleClip subtitleClip = new SubtitleClip
            {
                Id = clipping.Id,
                IsSelected = true,

                Subtitles = this.Subtitles.ToList(),
            };

            subtitleClip.Text = SubtitleClip.GetIndex(subtitleClip.Subtitles, position, this.Delay);
            subtitleClip.InitializeClipBase(isMuted, this.Delay, base.Index, trackHeight, trackScale);
            subtitleClip.InitializeFrameClip(nextDuration, trackScale);
            return subtitleClip;
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