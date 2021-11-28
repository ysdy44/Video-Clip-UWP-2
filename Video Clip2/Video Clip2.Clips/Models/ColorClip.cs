using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ColorClip : FrameClip, IClip
    {

        private readonly ColorSourceEffect Source;

        public override ClipType Type => ClipType.Color;
        public Color Color { get; }
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Black, Symbol.Flag);

        public ColorClip(Color color, bool isMuted, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.Color = color;
            this.Source = new ColorSourceEffect
            {
                Color = color
            };
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.Clear(this.Color);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
        {
            if (base.InRange(position) == false) return null;
            else return this.Source;
        }

        public override void SetPreviewSize(Size previewSize)
        {
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale, Size previewSize)
        {
            return new ColorClip(this.Color, isMuted, position, nextDuration, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            this.Source.Dispose();
        }

    }
}