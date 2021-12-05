using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Video_Clip2.Clips;
using Video_Clip2.Clips;
using Video_Clip2.Tools;
using Windows.UI.Xaml;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        public ToolType ToolType
        {
            get => this.toolType;
            set
            {
                this.toolType = value;
                this.OnPropertyChanged(nameof(ToolType)); // Notify 
            }
        }
        private ToolType toolType = ToolType.Cursor;


        public TimeSpan Position
        {
            get => this.position;
            set
            {
                this.position = value;
                this.OnPropertyChanged(nameof(Position)); // Notify 

                this.Invalidate(); // Invalidate
            }
        }
        private TimeSpan position = TimeSpan.Zero;


        public TimeSpan Duration
        {
            get => this.duration;
            set
            {
                this.duration = value;
                this.OnPropertyChanged(nameof(Duration)); // Notify  

                this.Invalidate(); // Invalidate
            }
        }
        private TimeSpan duration = TimeSpan.FromMinutes(1);


        public double TrackScale
        {
            get => this.trackScale;
            set
            {
                this.trackScale = value;
                this.OnPropertyChanged(nameof(TrackScale)); // Notify 

                foreach (IClip item in this.ObservableCollection)
                {
                    item.Track.SetLeft(value, item.Delay);
                    item.Track.SetWidth(value, item.Duration);
                }

                this.SetMode(); // Selection
            }
        }
        private double trackScale = 16d;


        public double TrackHeight
        {
            get => this.trackHeight;
            set
            {
                this.trackHeight = value;
                this.OnPropertyChanged(nameof(TrackHeight)); // Notify

                foreach (IClip item in this.ObservableCollection)
                {
                    item.Track.SetTop(item.Index, value);
                    item.Track.SetHeight(value);
                }
            }
        }
        private double trackHeight = 50d;


        public TimeSpan GetPosition(TimeSpan position)
        {
            if (position <= TimeSpan.Zero)
                return TimeSpan.Zero;

            if (position >= this.Duration)
                return this.Duration;

            foreach (TimeSpan item in this.PinCollection)
            {
                TimeSpan time = item;
                {
                    double distance = (time - position).ToDouble(this.TrackScale);
                    if (distance > -20 && distance < 20)
                    {
                        return time;
                    }
                }
            }

            foreach (IClip item in this.ObservableCollection)
            {
                {
                    TimeSpan time = item.Delay;
                    {
                        double distance = (time - position).ToDouble(this.TrackScale);
                        if (distance > -20 && distance < 20)
                        {
                            return time;
                        }
                    }
                }
                {
                    TimeSpan time = item.Delay + item.Duration;
                    {
                        double distance = (time - position).ToDouble(this.TrackScale);
                        if (distance > -20 && distance < 20)
                        {
                            return time;
                        }
                    }
                }
            }

            return position;
        }


        public readonly ObservableCollection<TimeSpan> PinCollection = new ObservableCollection<TimeSpan>
        {
        };
        public readonly ObservableCollection<IClip> ObservableCollection = new ObservableCollection<IClip>
        {
        };

        private readonly TimeSpan SpeedInterval = TimeSpan.FromMilliseconds(25);
        private readonly DispatcherTimer Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(25)
        };


        public bool IsPlaying
        {
            get => this.isPlaying;
            set
            {
                this.isPlaying = value;
                this.OnPropertyChanged(nameof(IsPlaying)); // Notify 
                this.IsPlayingCore = value;
            }
        }
        private bool isPlaying;

        /// <summary> Gets or sets the state of playback. </summary>
        public bool IsPlayingCore
        {
            get => this.isPlayingCore;
            set
            {
                if (value)
                    this.Timer.Start();
                else
                    this.Timer.Stop();

                this.isPlayingCore = value;

                this.Invalidate(); // Invalidate
            }
        }
        private bool isPlayingCore;


        public bool IsMuted
        {
            get => this.isMuted;
            set
            {
                this.isMuted = value;
                this.OnPropertyChanged(nameof(IsMuted)); // Notify 

                foreach (IClip item in this.ObservableCollection)
                {
                    item.SetIsMuted(value, item.IsMuted);
                }
            }
        }
        private bool isMuted;


        public ViewModel()
        {
            // Tick
            this.Timer.Tick += (s, e) =>
            {
                if (this.Duration == TimeSpan.Zero || this.Position >= this.Duration)
                    this.IsPlaying = false;

                this.Position += this.SpeedInterval;
            };
        }


        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None)
        {
            foreach (CanvasControl item in this.Canvas)
            {
                item.Invalidate();
            }
        }

        public readonly IList<CanvasControl> Canvas = new List<CanvasControl>();


        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}