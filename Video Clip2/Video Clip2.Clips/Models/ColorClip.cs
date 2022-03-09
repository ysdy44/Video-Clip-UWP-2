using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.ClipTracks;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ColorClip : FrameClip, IClip
    {

        public Color Color { get; set; }
        private ColorSourceEffect Source;
        public Transformer Transformer { get; private set; }

        public override ClipType Type => ClipType.Color;
        public override bool IsOverlayLayer => false;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Black, Symbol.Flag);

        public Transformer GetActualTransformer() => this.Transformer;

        public void Initialize(bool isMuted, BitmapSize size, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
        {
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeFrameClip(duration, trackScale);
            this.InitializeColorClip(size);
        }
        protected void InitializeColorClip(BitmapSize size)
        {
            uint width = size.Width;
            uint height = size.Height; 
            this.Transformer = new Transformer(width, height, Vector2.Zero);
            this.Source = new ColorSourceEffect
            {
                Color = this.Color
            };
        }


        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.Clear(this.Color);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Matrix3x2 matrix)
        {
            if (base.InRange(position) == false) return null;
            else return this.Source;
        }

        public void SetColor(Color color)
        {
            this.Color = color;
            this.Source = new ColorSourceEffect
            {
                Color = this.Color
            };

            this.Track.Invalidate(); // Invalidate
        }

        protected override IClip TrimClone(Clipping clipping, bool isMuted, BitmapSize size, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
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
            colorClip.InitializeColorClip(size);
            return colorClip;
        }

        public void Dispose()
        {
            this.Source.Dispose();
        }

    }
}