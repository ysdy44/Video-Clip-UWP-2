using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;
using Windows.Storage;

namespace Video_Clip2.Medias
{
    public abstract class Media
    {

        //@Property
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Token { get; set; }

        //@Abstract
        public abstract Task ConstructSource(ICanvasResourceCreator resourceCreator, StorageFile file);

        public Medium ToMedium()
        {
            return new Medium
            {
                Name = this.Name,
                FileType = this.FileType,
                Token = this.Token,
            };
        }

    }
}