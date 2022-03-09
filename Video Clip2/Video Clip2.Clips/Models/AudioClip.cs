using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Medias;
using Video_Clip2.Medias.Models;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class AudioClip : MediaClip, IClip
    {

        public Medium Medium { get; set; }

        public override ClipType Type => ClipType.Audio;
        public override bool IsOverlayLayer => false;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Fuchsia, Symbol.Audio);

        public void Initialize(double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, int index, double trackHeight, double trackScale)
        {
            Audio audio = Audio.Instances[this.Medium.Token];
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeMediaClip(audio.CreateSource(), playbackRate, isMuted, position, audio.Duration, trackScale);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Matrix3x2 matrix)
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

            return null;
        }

        protected override IClip TrimClone(Clipping clipping, double playbackRate, bool isMuted, BitmapSize size, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            // Clip
            AudioClip audioClip = new AudioClip
            {
                Id = clipping.Id,
                IsSelected = true,

                TrimTimeFromStart = nextTrimTimeFromStart,
                TrimTimeFromEnd = trimTimeFromEnd,

                Medium = this.Medium
            };

            Audio audio = Audio.Instances[audioClip.Medium.Token];
            audioClip.InitializeClipBase(isMuted, position, base.Index, trackHeight, trackScale);
            audioClip.InitializeMediaClip(audio.CreateSource(), playbackRate, isMuted, position, audio.Duration, trackScale);
            return audioClip;
        }

        public void Dispose()
        {
            base.Player.Pause();
            base.Player.Dispose();
        }

    }
}