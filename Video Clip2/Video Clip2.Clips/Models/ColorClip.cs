using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ColorClip : FrameClip, IClip
    {

        public Color Color { get; set; }
        private ColorSourceEffect Source;

        public override ClipType Type => ClipType.Color;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Black, Symbol.Flag);

        public void Initialize(bool isMuted, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
        {
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeFrameClip(duration, trackScale);
            this.InitializeColorClip();
        }
        protected void InitializeColorClip()
        {
            this.Source = new ColorSourceEffect
            {
                Color = this.Color
            };
        }


        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.Clear(this.Color);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (base.InRange(position) == false) return null;
            else return this.Source;
        }

        public void SetColor(Color color)
        {
            this.Color = color;
            this.InitializeColorClip();

            this.Track.Invalidate(); // Invalidate
        }

        protected override IClip TrimClone(Clipping clipping, bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            // Clip
            ColorClip colorClip = new ColorClip
            {
                Id = clipping.Id,
                IsSelected = true,

                Color = this.Color
            };

            colorClip.InitializeClipBase(isMuted, position, base.Index, trackHeight, trackScale);
            colorClip.InitializeFrameClip(nextDuration, trackScale);
            colorClip.InitializeColorClip();
            return colorClip;
        }

        public void Dispose()
        {
            this.Source.Dispose();
        }

    }
}