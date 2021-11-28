using System;
using Video_Clip2.Clips.Clips;
using Windows.Foundation;
using Windows.Media.Playback;

namespace Video_Clip2.Clips
{
    public abstract class MediaClip : ClipBase
    {

        //@Abstract
        protected abstract IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextTrimTimeFromStart, TimeSpan trimTimeFromEnd, double trackHeight, double trackScale, Size previewSize);

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

        protected MediaClip(MediaPlayer player, bool isMuted, TimeSpan delay, TimeSpan originalDuration, TimeSpan timTimeFromStart, TimeSpan trimTimeFromEnd, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, index, trackHeight, trackScale)
        {
            this.OriginalDuration = originalDuration;
            this.TrimTimeFromStart = timTimeFromStart;
            this.TrimTimeFromEnd = trimTimeFromEnd;
            this.Player = player;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public override void SetIsMuted(bool value, bool isMuted)
        {
            base.SetIsMuted(value, isMuted);
            this.Player.IsMuted = value || isMuted;
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

        public override void TrimDuration(double trackScale, TrimmerValue destinationValue, TrimmerValue sourceValue, double offset)
        {
            double destination = destinationValue.Value;
            double source = sourceValue.Value;

            double scale = (destination - sourceValue.GetValue(offset)) / (destination - source);

            double start = destination - (destination - this.StartingDelay) * scale;
            double end = destination - (destination - this.StartingDelay - this.StartingTrimmedDuration) * scale;

            TimeSpan delay = start.ToTimeSpan(trackScale);
            this.Delay = delay;
            this.Track.SetLeft(trackScale, this.Delay);

            double distance = this.StartingOriginalDuration - (end - start);
            if (distance < 0) distance = 0;

            if (this.StartingTrimTimeFromStart == this.StartingTrimTimeFromEnd)
            {
                this.TrimTimeFromStart =
                this.TrimTimeFromEnd =
                (distance / 2).ToTimeSpan(trackScale);
            }
            else if (this.StartingTrimTimeFromStart == 0)
            {
                this.TrimTimeFromEnd = distance.ToTimeSpan(trackScale);
            }
            else if (this.StartingTrimTimeFromEnd == 0)
            {
                this.TrimTimeFromStart = distance.ToTimeSpan(trackScale);
            }
            else
            {
                double sum = this.StartingTrimTimeFromStart + this.StartingTrimTimeFromEnd;
                this.TrimTimeFromStart = (distance * this.StartingTrimTimeFromStart / sum).ToTimeSpan(trackScale);
                this.TrimTimeFromEnd = (distance * this.StartingTrimTimeFromEnd / sum).ToTimeSpan(trackScale);
            }

            this.Track.SetWidth(trackScale, this.TrimmedDuration);
        }

        public IClip TrimClone(bool isMuted, TimeSpan position, double trackHeight, double trackScale, Size previewSize)
        {
            TimeSpan trimTimeFromStart = this.TrimTimeFromStart;
            TimeSpan trimTimeFromEnd = this.TrimTimeFromEnd;

            TimeSpan lastTrimTimeFromEnd = base.Delay - trimTimeFromStart + this.OriginalDuration - position;
            TimeSpan nextTrimTimeFromStart = position - Delay + trimTimeFromStart;

            this.TrimTimeFromEnd = lastTrimTimeFromEnd;
            this.Track.SetWidth(trackScale, this.TrimmedDuration);

            return this.TrimClone(isMuted, position, nextTrimTimeFromStart, trimTimeFromEnd, trackHeight, trackScale, previewSize);
        }

    }
}