using System;
using Video_Clip2.Clips.Clips;
using Windows.Foundation;

namespace Video_Clip2.Clips
{
    public abstract class FrameClip : ClipBase
    {

        //@Abstract
        protected abstract IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale);

        public override TimeSpan Duration => this.CoreDuration;
        protected TimeSpan CoreDuration;
        double StartingDuration;

        protected FrameClip(bool isMuted, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
                : base(isMuted, delay, index, trackHeight, trackScale)
        {
            this.CoreDuration = duration;
            this.Track.SetWidth(trackScale, duration);
        }

        public override bool InRange(TimeSpan position)
        {
            if (position < this.Delay) return false;
            if (position > this.Delay + this.Duration) return false;
            return true;
        }

        public void ChangeView(TimeSpan position, TimeSpan delay, TimeSpan duration)
        {
        }

        public void ChangeSize(double trackScale, TimeSpan delay, TimeSpan duration)
        {
            if (this.CoreDuration != duration)
            {
                this.CoreDuration = duration;
                this.Track.SetWidth(trackScale, this.Duration);
            }

            if (base.Delay != delay)
            {
                base.Delay = delay;
                this.Track.SetLeft(trackScale, base.Delay);
            }
        }

        public override void CacheDuration(double trackScale)
        {
            this.StartingDuration = this.Duration.ToDouble(trackScale);
        }

        public override void TrimDuration(double trackScale, TrimmerValue destinationValue, TrimmerValue sourceValue, double offset)
        {
            double destination = destinationValue.Value;
            double source = sourceValue.Value;

            double scale = (destination - sourceValue.GetValue(offset)) / (destination - source);

            double start = destination - (destination - this.StartingDelay) * scale;
            double end = destination - (destination - this.StartingDelay - this.StartingDuration) * scale;

            TimeSpan delay = start.ToTimeSpan(trackScale);
            this.Delay = delay;
            this.Track.SetLeft(trackScale, this.Delay);

            TimeSpan duration = (end - start).ToTimeSpan(trackScale);
            this.CoreDuration = duration;
            this.Track.SetWidth(trackScale, this.Duration);
        }
        public IClip TrimClone(bool isMuted, TimeSpan position, double trackHeight, double trackScale)
        {
            TimeSpan lastDuration = position - base.Delay;
            TimeSpan nextDuration = base.Delay + this.Duration - position;

            this.CoreDuration = lastDuration;
            this.Track.SetWidth(trackScale, this.Duration);

            return this.TrimClone(isMuted, position, nextDuration, trackHeight, trackScale);
        }

    }
}