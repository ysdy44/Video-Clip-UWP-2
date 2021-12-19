using Microsoft.Graphics.Canvas;
using Windows.Storage;

namespace Video_Clip2.Medias
{
    public sealed partial class Photo
    {

        //@Property
        public string Name { get; private set; }
        public string FileType { get; private set; }
        public string Token { get; private set; }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public IStorageFile File { get; private set; }
        public CanvasBitmap Bitmap { get; private set; }
        public CanvasBitmap Thumbnail { get; private set; }

        public Photocopier ToPhotocopier()
        {
            return new Photocopier
            {
                Name = this.Name,
                FileType = this.FileType,
                Token = this.Token,
            };
        }

    }
}