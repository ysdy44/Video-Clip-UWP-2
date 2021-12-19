using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Elements;
using Video_Clip2.Medias.Models;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public partial class VideoClip : MediaClip, IClip, ITransform
    {

        readonly Video Video;
        readonly CanvasRenderTarget Bitmap;
        readonly RenderTransform TransformCore;

        public override ClipType Type => ClipType.Video;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.BlueViolet, Symbol.Video);
        public RenderTransform Transform => this.TransformCore;

        private VideoClip(Video video, double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(video.CreateSource(), playbackRate, isMuted, position, delay, originalDuration, timTimeFromStart, trimTimeFromEnd, index, trackHeight, trackScale)
        {
            this.Video = video;
            this.Bitmap = new CanvasRenderTarget(ClipManager.CanvasDevice, this.Video.Width, this.Video.Height, 96);
            this.TransformCore = new RenderTransform(this.Video.Width, this.Video.Height);

            base.Player.IsVideoFrameServerEnabled = true;
            base.Player.VideoFrameAvailable += (s, e) =>
            {
                base.Player.CopyFrameToVideoSurface(this.Bitmap);
            };
        }
        public VideoClip(Video video, bool isMuted, TimeSpan position, TimeSpan delay, int index, double trackHeight, double trackScale)
            : this(video, 1, isMuted, position, delay, video.Duration, TimeSpan.Zero, TimeSpan.Zero, index, trackHeight, trackScale)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            double width = sender.ActualWidth;
            double position = base.PlaybackRate * base.TrimTimeFromStart.TotalSeconds;
            double lenth = base.PlaybackRate * base.TrimmedDuration.TotalSeconds;

            this.Video.DrawThumbnails(args.DrawingSession, width, position, lenth);
        }

        public ICanvasImage GetPlayerRender(TimeSpan position, Size previewSize)
        {
            base.Player.PlaybackSession.Position = position.Scale(this.PlaybackRate);

            if (base.IsPlaying)
            {
                base.Player.Pause();
            }

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

                    base.Player.Play();
                }
            }
            else
            {
                base.Player.PlaybackSession.Position = base.GetSpeedPlayerPosition(position);

                if (base.IsPlaying)
                {
                    base.Player.Pause();
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
            return new VideoClip(this.Video, playbackRate, isMuted, position, position, base.OriginalDuration, nextTrimTimeFromStart, trimTimeFromEnd, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
            base.Player.Pause();
            base.Player.Dispose();
        }

    }
}