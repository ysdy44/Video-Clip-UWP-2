using System;
using Video_Clip2.Elements;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace Video_Clip2.Clips
{
    public abstract class MediaClip : ClipBase
    {

        //@Abstract
        protected abstract IClip TrimClone(double playbackRate, bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale);

        public override TimeSpan Duration => this.TrimmedDuration;

        public readonly TimeSpan OriginalDuration;
        protected TimeSpan TrimmedDuration => this.SpeedDuration - this.TrimTimeFromStart - this.TrimTimeFromEnd;
        protected TimeSpan SpeedDuration { get; private set; }
        public TimeSpan TrimTimeFromStart { get; private set; }
        public TimeSpan TrimTimeFromEnd { get; private set; }

        protected double StartingOriginalDuration;
        protected double StartingTrimmedDuration;
        protected double StartingSpeedDuration;
        protected double StartingTrimTimeFromStart;
        protected double StartingTrimTimeFromEnd;

        protected readonly MediaPlayer Player;
        protected bool IsPlaying => this.Player.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;
        public double PlaybackRate => this.Player.PlaybackSession.PlaybackRate;
        public double Volume => this.Player.Volume;

        protected MediaClip(IMediaPlaybackSource source, double playbackRate, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, index, trackHeight, trackScale)
        {
            this.Player = new MediaPlayer
            {
                Source = source,
                IsMuted = isMuted
            };
            this.Player.PlaybackSession.PlaybackRate = playbackRate;
            this.Player.PlaybackSession.Position = this.GetSpeedPlayerPosition(position);

            this.OriginalDuration = originalDuration;
            this.SpeedDuration = playbackRate == 1 ? originalDuration : (originalDuration.ToDouble() / playbackRate).ToTimeSpan();
            this.TrimTimeFromStart = timTimeFromStart;
            this.TrimTimeFromEnd = trimTimeFromEnd;

            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public void SetPlaybackRate(double value, double trackScale)
        {
            this.Player.PlaybackSession.PlaybackRate = value;

            double newSpeedDuration = this.StartingOriginalDuration / value;
            this.SpeedDuration = newSpeedDuration.ToTimeSpan(trackScale);

            double scale = newSpeedDuration / this.StartingSpeedDuration;
            this.TrimTimeFromStart = (this.StartingTrimTimeFromStart * scale).ToTimeSpan(trackScale);
            this.TrimTimeFromEnd = (this.StartingTrimTimeFromEnd * scale).ToTimeSpan(trackScale);

            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }
        public void SetVolume(double value)
        {
            this.Player.Volume = value;
        }
        public override void SetIsMuted(bool isMuted, bool isMutedCore)
        {
            base.SetIsMuted(isMuted, isMutedCore);
            this.Player.IsMuted = isMuted || isMutedCore;
        }

        protected TimeSpan GetSpeedPlayerPosition(TimeSpan position)
        {
            TimeSpan playerPosition = position - (base.Delay - this.TrimTimeFromStart);
            TimeSpan speedPlayerPosition = playerPosition.Scale(this.PlaybackRate);
            return speedPlayerPosition;
        }

        public void SetDuration(double trackScale, TimeSpan trimTimeFromStart, TimeSpan trimTimeFromEnd)
        {
            this.TrimTimeFromStart = trimTimeFromStart;
            this.TrimTimeFromEnd = trimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public override void CacheDuration(double trackScale)
        {
            this.StartingOriginalDuration = this.OriginalDuration.ToDouble(trackScale);
            this.StartingSpeedDuration = this.SpeedDuration.ToDouble(trackScale);
            this.StartingTrimmedDuration = this.TrimmedDuration.ToDouble(trackScale);
            this.StartingTrimTimeFromStart = this.TrimTimeFromStart.ToDouble(trackScale);
            this.StartingTrimTimeFromEnd = this.TrimTimeFromEnd.ToDouble(trackScale);
        }

        public override void TrimStart(double trackScale, double offset, TimeSpan position)
        {
            double minDuration = TimeSpan.FromSeconds(2).ToDouble(trackScale);
            double innerStart = this.StartingDelay;
            double start = innerStart - this.StartingTrimTimeFromStart;
            double end = start + this.StartingOriginalDuration;
            double innerEnd = end - this.StartingTrimTimeFromEnd;

            double newInnerStart = innerStart + offset;
            if (newInnerStart < Math.Max(0, start)) newInnerStart = Math.Max(0, start);
            if (newInnerStart > innerEnd - minDuration) newInnerStart = innerEnd - minDuration;

            TimeSpan delay = newInnerStart.ToTimeSpan(trackScale);
            this.Delay = delay;
            this.Track.SetLeft(trackScale, this.Delay);

            TimeSpan trimTimeFromEnd = (newInnerStart - start).ToTimeSpan(trackScale);
            this.TrimTimeFromStart = trimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }
        public override void TrimEnd(double trackScale, double offset, TimeSpan position)
        {
            double minDuration = TimeSpan.FromSeconds(2).ToDouble(trackScale);
            double innerStart = this.StartingDelay;
            double start = innerStart - this.StartingTrimTimeFromStart;
            double end = start + this.StartingOriginalDuration;
            double innerEnd = end - this.StartingTrimTimeFromEnd;

            double newInnerEnd = innerEnd + offset;
            if (newInnerEnd < innerStart + minDuration) newInnerEnd = innerStart + minDuration;
            if (newInnerEnd > end) newInnerEnd = end;

            TimeSpan trimTimeFromEnd = (end - newInnerEnd).ToTimeSpan(trackScale);
            this.TrimTimeFromEnd = trimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public IClip TrimClone(bool isMuted, TimeSpan position, double trackHeight, double trackScale)
        {
            TimeSpan trimTimeFromStart = this.TrimTimeFromStart;
            TimeSpan trimTimeFromEnd = this.TrimTimeFromEnd;

            TimeSpan lastTrimTimeFromEnd = base.Delay - trimTimeFromStart + this.SpeedDuration - position;
            TimeSpan nextTrimTimeFromStart = position - base.Delay + trimTimeFromStart;

            this.TrimTimeFromEnd = lastTrimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);

            return this.TrimClone(this.PlaybackRate, isMuted, position, nextTrimTimeFromStart, trimTimeFromEnd, trackHeight, trackScale);
        }

    }
}