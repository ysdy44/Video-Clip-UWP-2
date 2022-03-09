using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void SetMode()
        {
            // Clips
            int count = this.ObservableCollection.Count(c => c.Self.IsSelected);

            switch (count)
            {
                case 0:
                    this.SetModeNone(); // None
                    break;
                case 1:
                    IClip clip = this.ObservableCollection.First(c => c.Self.IsSelected).Self;
                    this.SetModeSingle(clip); // Single
                    break;
                default:
                    IEnumerable<IClip> clips = this.ObservableCollection.Where(c => c.Self.IsSelected).Select(c => c.Self);
                    this.SetModeMultiple(clips); // Multiple
                    break;
            }
        }


        //////////////////////////


        public void SetModeNone()
        {
            this.SelectionMode = ListViewSelectionMode.None;
        }


        //////////////////////////


        public void SetModeSingle(IClip clip)
        {
            this.SelectionMode = ListViewSelectionMode.Single;

            this.Transformer = clip.GetActualTransformer();

            this.Trimmer = new Trimmer
            {
                Top = clip.Index,
                MinTop = 0,
                MaxTop = 5,

                Bottom = clip.Index,
                MinBottom = 0,
                MaxBottom = 5,

                Left = new TrimmerValue
                {
                    Value = clip.Delay.ToDouble(this.TrackScale),
                    MinValue = 0,
                    MaxValue = (clip.Delay + clip.Duration - TimeSpan.FromSeconds(1)).ToDouble(this.TrackScale),
                },

                Right = new TrimmerValue
                {
                    Value = (clip.Delay + clip.Duration).ToDouble(this.TrackScale),
                    MinValue = (clip.Delay + TimeSpan.FromSeconds(1)).ToDouble(this.TrackScale),
                    MaxValue = double.MaxValue,
                }
            };
        }


        //////////////////////////


        public void SetModeMultiple(IEnumerable<IClip> clips)
        {
            this.SelectionMode = ListViewSelectionMode.Multiple;

            // TransformerBorder
            TransformerBorder border = new TransformerBorder(clips);
            this.Transformer = border.ToTransformer();

            this.Trimmer = new Trimmer
            {
                Top = clips.Min(c => c.Index),
                MinTop = 0,
                MaxTop = 5,

                Bottom = clips.Max(c => c.Index),
                MinBottom = 0,
                MaxBottom = 5,

                Left = new TrimmerValue
                {
                    Value = clips.Min(c => c.Delay).ToDouble(this.TrackScale),
                    MinValue = 0,
                    MaxValue = (clips.Min(c => c.Delay + c.Duration) - TimeSpan.FromSeconds(1)).ToDouble(this.TrackScale),
                },

                Right = new TrimmerValue
                {
                    Value = clips.Max(c => c.Delay + c.Duration).ToDouble(this.TrackScale),
                    MinValue = (clips.Min(c => c.Delay) + TimeSpan.FromSeconds(1)).ToDouble(this.TrackScale),
                    MaxValue = double.MaxValue,
                }
            };
        }

    }
}