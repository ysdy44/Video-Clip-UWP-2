using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class ImageClip : FrameClip, IClip, ITransform
    {

        readonly RenderTransform TransformCore;
        readonly CanvasBitmap Bitmap;
        readonly CanvasBitmap Thumbnail;

        public override ClipType Type => ClipType.Image;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.DodgerBlue, Symbol.Pictures);
        public RenderTransform Transform => this.TransformCore;

        public ImageClip(CanvasBitmap bitmap, CanvasBitmap thumbnail, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            uint width = bitmap.SizeInPixels.Width;
            uint height = bitmap.SizeInPixels.Height;
            this.TransformCore = new RenderTransform(width, height);
            this.Bitmap = bitmap;
            this.Thumbnail = thumbnail;
            base.ChangeView(position, delay, duration);
        }
        public ImageClip(CanvasBitmap bitmap, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : this(bitmap, ImageClip.LoadThumbnail(bitmap), isMuted, position, delay, duration, index, trackHeight, trackScale)
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

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (base.InRange(position) == false) return null;

            this.Transform.ReloadMatrix(previewSize);
            return new Transform2DEffect
            {
                TransformMatrix = this.Transform.Matrix,
                Source = this.Bitmap
            };
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            return new ImageClip(this.Bitmap, this.Thumbnail, isMuted, position, position, nextDuration, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
        }

        //@Static
        public static async Task<CanvasBitmap> LoadBitmapAsync(IStorageFile item)
        {
            using (IRandomAccessStream stream = await item.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(ClipManager.CanvasDevice, stream);
                return bitmap;
            }
        }
        public static CanvasBitmap LoadThumbnail(CanvasBitmap bitmap)
        {
            uint width = bitmap.SizeInPixels.Width;
            uint height = bitmap.SizeInPixels.Height;

            const int scaledHeight = 50;
            int scaledWidth = (int)(scaledHeight * width / height);

            float scaleX = 1f * scaledWidth / width;
            float scaleY = 1f * scaledHeight / height;
            float scale = Math.Max(0.01f, Math.Max(scaleX, scaleY));

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(ClipManager.CanvasDevice, scaledWidth, scaledHeight, 96);
            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.DrawImage(new ScaleEffect
                {
                    Source = bitmap,
                    InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                    Scale = new Vector2(scale)
                });
            }
            return renderTarget;
        }

    }
}