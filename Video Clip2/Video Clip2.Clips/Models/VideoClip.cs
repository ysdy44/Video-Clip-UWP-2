using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Xml.Linq;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Elements;
using Video_Clip2.Medias;
using Video_Clip2.Medias.Models;
using Video_Clip2.Transforms;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public partial class VideoClip : MediaClip, IClip, ITransform
    {

        public Medium Medium { get; set; }
        CanvasRenderTarget Bitmap;
        public RenderTransform Transform { get; private set; }

        public override ClipType Type => ClipType.Video;
        public override IClipTrack Track { get; } = new LazyClipTrack(Colors.BlueViolet, Symbol.Video);

        public void Initialize(double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, int index, double trackHeight, double trackScale)
        {
            Video video = Video.Instances[this.Medium.Token];
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeMediaClip(video.CreateSource(), playbackRate, isMuted, position, video.Duration, trackScale);
            this.InitializeVideoClip(video);
        }
        protected void InitializeVideoClip(Video video)
        {
            this.Bitmap = new CanvasRenderTarget(ClipManager.CanvasDevice, video.Width, video.Height, 96);
            this.Transform = new RenderTransform(video.Width, video.Height);
            base.Player.IsVideoFrameServerEnabled = true;
            base.Player.VideoFrameAvailable += (s, e) =>
            {
                base.Player.CopyFrameToVideoSurface(this.Bitmap);
            };
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            double width = sender.ActualWidth;
            double position = base.PlaybackRate * base.TrimTimeFromStart.TotalSeconds;
            double lenth = base.PlaybackRate * base.TrimmedDuration.TotalSeconds;

            Video video = Video.Instances[this.Medium.Token];
            video.DrawThumbnails(args.DrawingSession, width, position, lenth);
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

        protected override IClip TrimClone(Clipping clipping, double playbackRate, bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            // Clip
            VideoClip videoClip = new VideoClip
            {
                Id = clipping.Id,
                IsSelected = true,

                TrimTimeFromStart = nextTrimTimeFromStart,
                TrimTimeFromEnd = trimTimeFromEnd,

                Medium = this.Medium
            };

            Video video = Video.Instances[videoClip.Medium.Token];
            videoClip.InitializeClipBase(isMuted, position, base.Index, trackHeight, trackScale);
            videoClip.InitializeMediaClip(video.CreateSource(), playbackRate, isMuted, position, video.Duration, trackScale);
            videoClip.InitializeVideoClip(video);
            return videoClip;
        }

        public void Dispose()
        {
            this.Bitmap.Dispose();
            base.Player.Pause();
            base.Player.Dispose();
        }

    }
}