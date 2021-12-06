﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class AudioClip : MediaClip, IClip
    {

        public override ClipType Type => ClipType.Audio;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Fuchsia, Symbol.Audio);

        private AudioClip(IMediaPlaybackSource source, double playbackRate, bool isMuted, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(new MediaPlayer { Source = source, IsMuted = isMuted }, playbackRate, isMuted, delay, originalDuration, timTimeFromStart, trimTimeFromEnd, index, trackHeight, trackScale)
        {
        }
        public AudioClip(IStorageFile file, bool isMuted, TimeSpan delay, TimeSpan originalDuration, int index, double trackHeight, double trackScale)
            : this(MediaSource.CreateFromStorageFile(file), 1, isMuted, delay, originalDuration, TimeSpan.Zero, TimeSpan.Zero, index, trackHeight, trackScale)
        {
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
        {
            if (base.InRange(position) == false)
            {
                if (this.IsPlaying) this.Player.Pause();
                return null;
            }

            base.SetPlayer(isPlaying, position);

            return null;
        }

        protected override IClip TrimClone(double playbackRate, bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale)
        {
            return new AudioClip(base.Player.Source, playbackRate, isMuted, position, base.OriginalDuration, nextTrimTimeFromStart, trimTimeFromEnd, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            base.Player.Pause();
            base.Player.Dispose();
        }

    }
}