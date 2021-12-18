using System;

namespace Video_Clip2.Clips
{
    public partial class Clipping
    {

        //@Static
        public static Clipping CreateByGuid()
        {
            return new Clipping
            {
                Id = Guid.NewGuid().ToString()
            };
        }

        public IClip Self => ClipBase.FindFirstClip(this);

        public string Id { get; set; }

        public Clipping Clone()
        {
            return new Clipping
            {
                Id = this.Id,
            };
        }
    }
}