﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Video_Clip2.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public partial class VideoClip : MediaClip, IClip
    {

        readonly uint Width;
        readonly uint Height;
        readonly IList<CanvasBitmap> Thumbnails;
        readonly ICanvasResourceCreatorWithDpi ResourceCreator;

        CanvasBitmap Bitmap;
        bool VideoFrameAvailable;

        public override ClipType Type => ClipType.Video;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.BlueViolet, Symbol.Video);

        public VideoClip(IMediaPlaybackSource source, uint width, uint height, IList<CanvasBitmap> thumbnails, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale, ICanvasResourceCreatorWithDpi resourceCreator)
            : base(new MediaPlayer { IsVideoFrameServerEnabled = true, Source = source, IsMuted = isMuted }, isMuted, delay, originalDuration, timTimeFromStart, trimTimeFromEnd, index, trackHeight, trackScale)
        {
            this.Width = width;
            this.Height = height;
            this.Thumbnails = thumbnails;
            this.ResourceCreator = resourceCreator;

            this.Bitmap = new CanvasRenderTarget(resourceCreator, width, height);

            base.Player.PlaybackSession.Position = position - base.Delay;
            base.Player.VideoFrameAvailable += (s, e) =>
            {
                this.VideoFrameAvailable = true;
            };
        }
        public VideoClip(IStorageFile file, uint width, uint height, IList<CanvasBitmap> thumbnails, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, int index, double trackHeight, double trackScale, ICanvasResourceCreatorWithDpi resourceCreator)
            : this(MediaSource.CreateFromStorageFile(file), width, height, thumbnails, isMuted, position, delay, originalDuration, TimeSpan.Zero, TimeSpan.Zero, index, trackHeight, trackScale, resourceCreator)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            double lenth = base.TrimmedDuration.TotalSeconds;
            double width = sender.ActualWidth;
            double position = base.TrimTimeFromStart.TotalSeconds;
            VideoClip.DrawThumbnails(this.Thumbnails, args.DrawingSession, lenth, width, position);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
        {
            if (base.InRange(position) == false) return null;

            if (isPlaying)
            {
                if (base.IsPlaying != isPlaying)
                {
                    base.Player.PlaybackSession.Position = position - (base.Delay - this.TrimTimeFromStart);
                    base.Player.Play();
                    base.IsPlaying = true;
                }
            }
            else
            {
                base.Player.PlaybackSession.Position = position - (base.Delay - this.TrimTimeFromStart);
                if (base.IsPlaying != isPlaying)
                {
                    base.Player.Pause();
                    base.IsPlaying = false;
                }
            }

            if (this.VideoFrameAvailable)
            {
                base.Player.CopyFrameToVideoSurface(this.Bitmap);
                this.VideoFrameAvailable = false;
            }
            return VideoClip.Render(this.Width, this.Height, this.Bitmap, previewSize);
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            return new VideoClip(base.Player.Source, this.Width, this.Height, this.Thumbnails, isMuted, position, position, base.OriginalDuration, nextTrimTimeFromStart, trimTimeFromEnd, base.Index, trackHeight, trackScale, this.ResourceCreator);
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
            base.Player.Pause();
            base.Player.Dispose();
        }


        //@Static
        public static void DrawThumbnails(IList<CanvasBitmap> thumbnails, CanvasDrawingSession drawingSession, double lenth, double width, double position)
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

        public static async Task<IList<CanvasBitmap>> LoadThumbnailsAsync(ICanvasResourceCreator resourceCreator, IStorageFile file, TimeSpan duration, uint width, uint height)
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
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, item);
                thumbnails2.Add(bitmap);
            }
            return thumbnails2;
        }

        private static ICanvasImage Render(uint width, uint height, CanvasBitmap bitmap, Size previewSize)
        {
            double scaleX = previewSize.Width / width;
            double scaleY = previewSize.Height / height;

            double scale =
                Math.Min(scaleX, scaleY);

            return new Transform2DEffect
            {
                TransformMatrix =
                   Matrix3x2.CreateTranslation(-width / 2, -height / 2) *
                   Matrix3x2.CreateScale(new Vector2((float)scale)) *
                   Matrix3x2.CreateTranslation((float)previewSize.Width / 2, (float)previewSize.Height / 2),
                Source = bitmap
            };
        }

    }
}