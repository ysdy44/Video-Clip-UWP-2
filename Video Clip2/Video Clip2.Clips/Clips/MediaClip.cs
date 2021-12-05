using System;
using Video_Clip2.Clips;
using Windows.Foundation;
using Windows.Media.Playback;

namespace Video_Clip2.Clips
{
    public abstract class MediaClip : ClipBase
    {

        //@Abstract
        protected abstract IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale);

        public override TimeSpan Duration => this.TrimmedDuration;
        protected TimeSpan TrimmedDuration => this.OriginalDuration - this.TrimTimeFromStart - this.TrimTimeFromEnd;
        protected TimeSpan OriginalDuration;
        protected TimeSpan TrimTimeFromStart;
        protected TimeSpan TrimTimeFromEnd;

        protected double StartingTrimmedDuration;
        protected double StartingOriginalDuration;
        protected double StartingTrimTimeFromStart;
        protected double StartingTrimTimeFromEnd;

        protected readonly MediaPlayer Player;
        protected bool IsPlaying;
        public double PlaybackRate => this.Player.PlaybackSession.PlaybackRate;
        public double Volume => this.Player.Volume;

        protected MediaClip(MediaPlayer player, bool isMuted, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, index, trackHeight, trackScale)
        {
            this.OriginalDuration = originalDuration;
            this.TrimTimeFromStart = timTimeFromStart;
            this.TrimTimeFromEnd = trimTimeFromEnd;
            this.Player = player;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public void SetPlaybackRate(double value)
        {
            this.Player.PlaybackSession.PlaybackRate = value;
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

        public override bool InRange(TimeSpan position)
        {
            if (position < this.Delay) return false;
            if (position > this.Delay + this.TrimmedDuration) return false;
            return true;
        }

        public override void CacheDuration(double trackScale)
        {
            this.StartingOriginalDuration = this.OriginalDuration.ToDouble(trackScale);
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

            TimeSpan lastTrimTimeFromEnd = base.Delay - trimTimeFromStart + this.OriginalDuration - position;
            TimeSpan nextTrimTimeFromStart = position - Delay + trimTimeFromStart;

            this.TrimTimeFromEnd = lastTrimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);

            return this.TrimClone(isMuted, position, nextTrimTimeFromStart, trimTimeFromEnd, trackHeight, trackScale);
        }

    }
}