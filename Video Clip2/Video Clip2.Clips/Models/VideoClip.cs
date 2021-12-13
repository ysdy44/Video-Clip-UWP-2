using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Video_Clip2.Clips.ClipManagers;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Elements;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Clips.Models
{
    public partial class VideoClip : MediaClip, IClip, ITransform
    {

        readonly RenderTransform TransformCore;
        readonly CanvasBitmap Bitmap;
        readonly IList<CanvasBitmap> Thumbnails;
        readonly DispatcherTimer Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(10)
        };

        public override ClipType Type => ClipType.Video;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.BlueViolet, Symbol.Video);
        public RenderTransform Transform => this.TransformCore;

        private VideoClip(IStorageFile file, uint width, uint height, IList<CanvasBitmap> thumbnails, double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(file, playbackRate, isMuted, position, delay, originalDuration, timTimeFromStart, trimTimeFromEnd, index, trackHeight, trackScale)
        {
            this.TransformCore = new RenderTransform(width, height);
            this.Bitmap = new CanvasRenderTarget(ClipManager.CanvasDevice, width, height, 96);
            this.Thumbnails = thumbnails;
            base.Player.VideoFrameAvailable += (s, e) =>
            {
                base.Player.CopyFrameToVideoSurface(this.Bitmap);
            };
            this.Timer.Tick += (s, e) =>
            {
                this.Timer.Stop();
                if (base.IsPlaying == false)
                {
                    // FrameServer
                    base.Player.IsVideoFrameServerEnabled = false;
                }
            };
        }
        public VideoClip(IStorageFile file, uint width, uint height, IList<CanvasBitmap> thumbnails, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, int index, double trackHeight, double trackScale)
            : this(file, width, height, thumbnails, 1, isMuted, position, delay, originalDuration, TimeSpan.Zero, TimeSpan.Zero, index, trackHeight, trackScale)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            double width = sender.ActualWidth;
            double position = base.PlaybackRate * base.TrimTimeFromStart.TotalSeconds;
            double lenth = base.PlaybackRate * base.TrimmedDuration.TotalSeconds;
            VideoClip.DrawThumbnails(this.Thumbnails, args.DrawingSession, width, position, lenth);
        }

        public ICanvasImage GetPlayerRender(TimeSpan position, Size previewSize)
        {
            base.Player.PlaybackSession.Position = position.Scale(this.PlaybackRate);

            if (base.IsPlaying)
            {
                base.Player.Pause();
            }

            // FrameServer
            this.Timer.Stop();
            this.Timer.Start();
            base.Player.IsVideoFrameServerEnabled = true;

            // Transform
            this.Transform.ReloadMatrix(previewSize);
            return new Transform2DEffect
            {
                TransformMatrix = this.Transform.Matrix,
                Source = this.Bitmap
            };
        }
        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (base.InRange(position) == false)
            {
                if (base.IsPlaying) base.Player.Pause();
                return null;
            }

            if (isPlaying)
            {
                if (base.IsPlaying == false)
                {
                    base.Player.PlaybackSession.Position = base.GetSpeedPlayerPosition(position);

                    base.Player.IsVideoFrameServerEnabled = true;
                    base.Player.Play();
                }
            }
            else
            {
                base.Player.PlaybackSession.Position = base.GetSpeedPlayerPosition(position);

                if (base.IsPlaying)
                {
                    base.Player.IsVideoFrameServerEnabled = false;
                    base.Player.Pause();
                }
                else
                {
                    // FrameServer
                    this.Timer.Stop();
                    this.Timer.Start();
                    base.Player.IsVideoFrameServerEnabled = true;
                }
            }

            // Transform
            this.Transform.ReloadMatrix(previewSize);
            return new Transform2DEffect
            {
                TransformMatrix = this.Transform.Matrix,
                Source = this.Bitmap
            };
        }

        protected override IClip TrimClone(double playbackRate, bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            return new VideoClip(base.File, this.Transform.Width, this.Transform.Height, this.Thumbnails, playbackRate, isMuted, position, position, base.OriginalDuration, nextTrimTimeFromStart, trimTimeFromEnd, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
            base.Player.Pause();
            base.Player.Dispose();
        }


        //@Static
        public static void DrawThumbnails(IList<CanvasBitmap> thumbnails, CanvasDrawingSession drawingSession, double width, double position, double lenth)
        {
            double offset = 0;

            do
            {
                double seconds = 0.5 + position + offset / width * lenth;
                int index = (int)Math.Round(seconds, MidpointRounding.AwayFromZero);

                CanvasBitmap bitmap = index < thumbnails.Count ? thumbnails[index] : thumbnails.Last();
                drawingSession.DrawImage(bitmap, (float)offset, 0);

                offset += bitmap.Size.Width;

            } while (offset < width);
        }

        public static async Task<IList<CanvasBitmap>> LoadThumbnailsAsync(IStorageFile file, TimeSpan duration, uint width, uint height)
        {
            Windows.Media.Editing.MediaComposition composition = new Windows.Media.Editing.MediaComposition
            {
                Clips =
                {
                    await Windows.Media.Editing.MediaClip.CreateFromFileAsync(file)
                }
            };

            const int scaledHeight = 50;
            int scaledWidth = (int)(scaledHeight * width / height);

            IEnumerable<TimeSpan> points = Enumerable.Range(0, 1 + (int)Math.Floor(duration.TotalSeconds)).Select(c => TimeSpan.FromSeconds(c));
            IReadOnlyList<ImageStream> thumbnails = await composition.GetThumbnailsAsync(points, scaledWidth, scaledHeight, Windows.Media.Editing.VideoFramePrecision.NearestFrame);

            IList<CanvasBitmap> thumbnails2 = new List<CanvasBitmap>();
            foreach (ImageStream item in thumbnails)
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(ClipManager.CanvasDevice, item);
                thumbnails2.Add(bitmap);
            }
            return thumbnails2;
        }

    }
}