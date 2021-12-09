using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Effects;
using Video_Clip2.Elements;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Video_Clip2.Clips
{
    public abstract partial class ClipBase
    {

        public abstract IClipTrack Track { get; }

        #region Instance

        public string Id { get; private set; }

        #endregion

        #region Property

        public abstract ClipType Type { get; }
        public string Name { get; set; }

        public float Opacity { get; set; } = 1;

        public Visibility Visibility { get; set; }
        public abstract bool InRange(TimeSpan position);

        public bool IsMuted { get; private set; }
        public virtual void SetIsMuted(bool isMuted, bool isMutedCore)
        {
            this.IsMuted = isMutedCore;
            this.Track.SetIsMuted(isMuted, isMutedCore);
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.isSelected = value;
                this.Track.SetisSelected(value);
            }
        }
        private bool isSelected = false;

        public Effect Effect { get; set; } = new Effect();

        #endregion

        #region Render

        public abstract void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args);
        public abstract ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize);

        int StartingIndex;
        public int Index { get; private set; }
        public void CacheIndex() => this.StartingIndex = this.Index;
        public void AddIndex(double trackHeight, int move)
        {
            int index = this.StartingIndex + move;

            if (this.Index != index)
            {
                this.Index = index;
                this.Track.SetTop(index, trackHeight);
            }
        }

        protected double StartingDelay;
        public TimeSpan Delay { get; protected set; }
        public void CacheDelay(double trackScale) => this.StartingDelay = this.Delay.ToDouble(trackScale);
        public void AddDelay(double trackScale, double offset, TimeSpan position)
        {
            double delay = this.StartingDelay + offset;
            TimeSpan delay2 = delay < 0 ? TimeSpan.Zero : delay.ToTimeSpan(trackScale);

            if (this.Delay != delay2)
            {
                this.Delay = delay2;
                this.Track.SetLeft(trackScale, delay2);
            }
        }

        public abstract TimeSpan Duration { get; }
        public abstract void CacheDuration(double trackScale);
        public abstract void TrimStart(double trackScale, double offset, TimeSpan position);
        public abstract void TrimEnd(double trackScale, double offset, TimeSpan position);

        #endregion

        protected ClipBase(bool isMuted, TimeSpan delay, int index, double trackHeight, double trackScale)
        {
            string id = Guid.NewGuid().ToString();
            this.Id = id;

            this.Track.SetIsMuted(isMuted, false);

            this.Delay = delay;
            this.Track.SetLeft(trackScale, delay);

            this.Index = index;
            this.Track.SetTop(index, trackHeight);
            this.Track.SetHeight(trackHeight);

            this.Track.TrackUnloaded += (s, e) => this.Track.Draw -= this.DrawThumbnail;
            this.Track.TrackLoaded += (s, e) => this.Track.Draw += this.DrawThumbnail;
        }

    }
}