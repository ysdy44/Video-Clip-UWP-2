using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Video_Clip2.Clips;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        public void MethodEditTrim()
        {
            this.IsPlaying = false;

            IEnumerable<Clipping> clippings = from c in this.ObservableCollection where c.Self.IsSelected select c;
            Clipping[] array = clippings.ToArray();
            if (array.Length <= 0) return;

            foreach (Clipping item in array)
            {
                IClip lastClip = item.Self;
                if (lastClip.InRange(this.Position, TimeSpan.FromSeconds(2)) == false) continue;

                //
                Clipping clipping = Clipping.CreateByGuid();
                IClip nextClip = lastClip.TrimClone(clipping, this.IsMuted, this.Size, this.Position, this.TrackHeight, this.TrackScale);
                if (nextClip == null) continue;
                ClipBase.Instances.Add(clipping.Id, nextClip);
                //

                this.ObservableCollection.Add(clipping);
                lastClip.IsSelected = false;
                nextClip.IsSelected = true;
            }

            this.SetMode(); // Selection
            this.Invalidate(); // Invalidate
        }
        public void MethodEditClear()
        {
            this.IsPlaying = false;

            IEnumerable<Clipping> clippings = from c in this.ObservableCollection where c.Self.IsSelected select c;
            Clipping[] array = clippings.ToArray();
            if (array.Length <= 0) return;

            foreach (Clipping item in array)
            {
                Clipping remove = item;
                this.ObservableCollection.Remove(remove);
            }

            this.SetModeNone(); // Selection
            this.Invalidate(); // Invalidate
        }

    }
}