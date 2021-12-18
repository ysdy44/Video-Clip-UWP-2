using System.Collections.Generic;

namespace Video_Clip2.Clips
{
    public abstract partial class ClipBase
    {

        //@Static
        public static readonly Dictionary<string, IClip> Instances = new Dictionary<string, IClip>();

        public static IClip FindFirstClip(Clipping clipping)
        {
            string id = clipping.Id;
            return ClipBase.Instances[id];
        }

        public string Id { get; set; }

        public bool Equals(Clipping other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }
        public bool Equals(ClipBase other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }

    }
}