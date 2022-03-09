using System;
using Video_Clip2.Elements;
using Windows.Graphics.Imaging;

namespace Video_Clip2.Clips
{
    public abstract class FrameClip : ClipBase
    {

        //@Abstract
        protected abstract IClip TrimClone(Clipping clipping, bool isMuted, BitmapSize size, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale);

        public override TimeSpan Duration => this.CoreDuration;
        protected TimeSpan CoreDuration;
        double StartingDuration;

        protected void InitializeFrameClip(TimeSpan duration, double trackScale)
        {
            this.CoreDuration = duration;
            this.Track.SetWidth(trackScale, duration);
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

        public override void TrimStart(double trackScale, double offset, TimeSpan position)
        {
            double minDuration = TimeSpan.FromSeconds(2).ToDouble(trackScale);
            double start = this.StartingDelay;
            double end = this.StartingDelay + this.StartingDuration;

            double newStart = start + offset;
            if (newStart < 0) newStart = 0;
            if (newStart > end - minDuration) newStart = end - minDuration;

            TimeSpan delay = newStart.ToTimeSpan(trackScale);
            this.Delay = delay;
            this.Track.SetLeft(trackScale, this.Delay);

            TimeSpan duration = (end - newStart).ToTimeSpan(trackScale);
            this.CoreDuration = duration;
            this.Track.SetWidth(trackScale, this.Duration);
        }
        public override void TrimEnd(double trackScale, double offset, TimeSpan position)
        {
            double minDuration = TimeSpan.FromSeconds(2).ToDouble(trackScale);
            double start = this.StartingDelay;
            double end = this.StartingDelay + this.StartingDuration;

            double newEnd = end + offset;
            if (newEnd < start + minDuration) newEnd = start + minDuration;

            TimeSpan duration = (newEnd - start).ToTimeSpan(trackScale);
            this.CoreDuration = duration;
            this.Track.SetWidth(trackScale, this.Duration);
        }
        public IClip TrimClone(Clipping clipping, bool isMuted, BitmapSize size, TimeSpan position, double trackHeight, double trackScale)
        {
            TimeSpan lastDuration = position - base.Delay;
            TimeSpan nextDuration = base.Delay + this.Duration - position;

            this.CoreDuration = lastDuration;
            this.Track.SetWidth(trackScale, this.Duration);

            return this.TrimClone(clipping, isMuted, size, position, nextDuration, trackHeight, trackScale);
        }

    }
}