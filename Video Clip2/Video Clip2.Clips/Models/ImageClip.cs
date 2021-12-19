using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Medias;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ImageClip : FrameClip, IClip, ITransform
    {

        readonly Photo Photo;
        readonly RenderTransform TransformCore;

        public override ClipType Type => ClipType.Image;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.DodgerBlue, Symbol.Pictures);
        public RenderTransform Transform => this.TransformCore;

        public ImageClip(Photo photo, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.Photo = photo;
            this.TransformCore = new RenderTransform(photo.Width, photo.Height);
            base.ChangeView(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(new BorderEffect
            {
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Clamp,
                Source = this.Photo.Thumbnail
            });
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (base.InRange(position) == false) return null;

            this.Transform.ReloadMatrix(previewSize);
            return new Transform2DEffect
            {
                TransformMatrix = this.Transform.Matrix,
                Source = this.Photo.Bitmap
            };
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            return new ImageClip(this.Photo, isMuted, position, position, nextDuration, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
        }

    }
}