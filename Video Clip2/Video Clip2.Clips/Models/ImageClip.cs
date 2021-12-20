using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Medias;
using Video_Clip2.Medias.Models;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ImageClip : FrameClip, IClip, IRenderTransform
    {

        public Medium Medium { get; set; }
        public RenderTransform RenderTransform { get; private set; }

        public override ClipType Type => ClipType.Image;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.DodgerBlue, Symbol.Pictures);

        public void Initialize(bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
        {
            Photo photo = Photo.Instances[this.Medium.Token];
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeFrameClip(duration, trackScale);
            this.InitializeImageClip(photo, position, delay, duration);
        }
        protected void InitializeImageClip(Photo photo, TimeSpan position, TimeSpan delay, TimeSpan duration)
        {
            this.RenderTransform = new RenderTransform(photo.Width, photo.Height);
            base.ChangeView(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            Photo photo = Photo.Instances[this.Medium.Token];
            args.DrawingSession.DrawImage(new BorderEffect
            {
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Clamp,
                Source = photo.Thumbnail
            });
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (base.InRange(position) == false) return null;

            Photo photo = Photo.Instances[this.Medium.Token];

            // Transform
            this.RenderTransform.ReloadMatrix(previewSize);
            return new Transform2DEffect
            {
                TransformMatrix = this.RenderTransform.Matrix,
                Source = photo.Bitmap
            };
        }

        protected override IClip TrimClone(Clipping clipping, bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            // Clip
            ImageClip imageClip = new ImageClip
            {
                Id = clipping.Id,
                IsSelected = true,

                Medium = this.Medium
            };

            Photo photo = Photo.Instances[this.Medium.Token];
            imageClip.InitializeClipBase(isMuted, position, base.Index, trackHeight, trackScale);
            imageClip.InitializeFrameClip(nextDuration, trackScale);
            imageClip.InitializeImageClip(photo, position, position, nextDuration);
            return imageClip;
        }

        public void Dispose()
        {
        }

    }
}