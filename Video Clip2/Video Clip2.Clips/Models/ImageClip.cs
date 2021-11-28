using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Clips.Models
{
    public class ImageClip : FrameClip, IClip
    {

        readonly CanvasBitmap Thumbnail;
        readonly CanvasBitmap Bitmap;
        ICanvasImage Image;
        public Stretch Stretch { get; private set; } = Stretch.Uniform;

        public override ClipType Type => ClipType.Image;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.DodgerBlue, Symbol.Pictures);

        public ImageClip(CanvasBitmap bitmap, CanvasBitmap thumbnail, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale, Size previewSize)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.Thumbnail = thumbnail;
            this.Bitmap = bitmap;
            this.Image = ImageClip.Rednder(this.Stretch, this.Bitmap, previewSize);
            base.ChangeView(position, delay, duration);
        }
        public ImageClip(CanvasBitmap bitmap, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
            : this(bitmap, ImageClip.LoadThumbnail(resourceCreator, bitmap), isMuted, position, delay, duration, index, trackHeight, trackScale, previewSize)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(new BorderEffect
            {
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Clamp,
                Source = this.Thumbnail
            });
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
        {
            if (base.InRange(position) == false) return null;
            else return this.Image;
        }

        public override void SetPreviewSize(Size previewSize)
        {
            this.Image = ImageClip.Rednder(this.Stretch, this.Bitmap, previewSize);
        }
        public void SetStretch(Stretch stretch, Size previewSize)
        {
            this.Stretch = stretch;
            this.Image = ImageClip.Rednder(stretch, this.Bitmap, previewSize);
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale, Size previewSize)
        {
            return new ImageClip(this.Bitmap, this.Thumbnail, isMuted, position, position, nextDuration, base.Index, trackHeight, trackScale, previewSize);
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
        }

        //@Static
        public static CanvasBitmap LoadThumbnail(ICanvasResourceCreatorWithDpi resourceCreator, CanvasBitmap bitmap)
        {
            uint width = bitmap.SizeInPixels.Width;
            uint height = bitmap.SizeInPixels.Height;

            const int scaledHeight = 50;
            int scaledWidth = (int)(scaledHeight * width / height);

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, scaledWidth, scaledHeight);
            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.DrawImage(new ScaleEffect
                {
                    Source = bitmap,
                    Scale = new Vector2(1f * scaledWidth / width, 1f * scaledHeight / height)
                });
            }
            return renderTarget;
        }

        private static ICanvasImage Rednder(Stretch stretch, CanvasBitmap bitmap, Size previewSize)
        {
            if (stretch == Stretch.None) return bitmap;

            double scaleX = previewSize.Width / bitmap.SizeInPixels.Width;
            double scaleY = previewSize.Height / bitmap.SizeInPixels.Height;
            if (stretch == Stretch.Fill) return new ScaleEffect
            {
                Scale = new Vector2((float)scaleX, (float)scaleY),
                Source = bitmap
            };

            double scale =
                stretch == Stretch.Uniform ?
                Math.Min(scaleX, scaleY) :
                Math.Max(scaleX, scaleY);

            return new Transform2DEffect
            {
                TransformMatrix =
                   Matrix3x2.CreateTranslation(-bitmap.SizeInPixels.Width / 2, -bitmap.SizeInPixels.Height / 2) *
                   Matrix3x2.CreateScale(new Vector2((float)scale)) *
                   Matrix3x2.CreateTranslation((float)previewSize.Width / 2, (float)previewSize.Height / 2),
                Source = bitmap
            };
        }

    }
}