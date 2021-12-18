using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Medias;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class AudioClip : MediaClip, IClip
    {

        readonly Audio Audio;

        public override ClipType Type => ClipType.Audio;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Fuchsia, Symbol.Audio);

        private AudioClip(Audio audio, double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(audio.CreateSource(), playbackRate, isMuted, position, delay, originalDuration, timTimeFromStart, trimTimeFromEnd, index, trackHeight, trackScale)
        {
            this.Audio = audio;
        }
        public AudioClip(Audio audio, bool isMuted, TimeSpan delay, int index, double trackHeight, double trackScale)
            : this(audio, 1, isMuted, delay, delay, audio.Duration, TimeSpan.Zero, TimeSpan.Zero, index, trackHeight, trackScale)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
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

            return null;
        }

        protected override IClip TrimClone(double playbackRate, bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            return new AudioClip(this.Audio, playbackRate, isMuted, position, position, base.OriginalDuration, nextTrimTimeFromStart, trimTimeFromEnd, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            base.Player.Pause();
            base.Player.Dispose();
        }

    }
}