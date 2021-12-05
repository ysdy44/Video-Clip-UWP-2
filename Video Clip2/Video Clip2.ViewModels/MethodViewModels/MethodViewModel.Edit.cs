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

            IEnumerable<string> ids = from c in this.ObservableCollection where c.IsSelected select c.Id;
            string[] array = ids.ToArray();
            if (array.Length <= 0) return;

            foreach (string item in array)
            {
                IClip lastClip = this.ObservableCollection.First(c => c.Id == item);
                if (lastClip.InRange(this.Position) == false) continue;

                IClip nextClip = lastClip.TrimClone(this.IsMuted, this.Position, this.TrackHeight, this.TrackScale);
                if (nextClip == null) continue;

                this.ObservableCollection.Add(nextClip);
                lastClip.IsSelected = false;
                nextClip.IsSelected = true;
            }

            this.SetMode(); // Selection
            this.Invalidate(); // Invalidate
        }
        public void MethodEditClear()
        {
            this.IsPlaying = false;

            IEnumerable<string> ids = from c in this.ObservableCollection where c.IsSelected select c.Id;
            string[] array = ids.ToArray();
            if (array.Length <= 0) return;

            foreach (string item in array)
            {
                IClip remove = this.ObservableCollection.First(c => c.Id == item);
                this.ObservableCollection.Remove(remove);
                remove.Dispose();
            }

            this.SetMode(); // Selection
            this.Invalidate(); // Invalidate
        }

    }
}