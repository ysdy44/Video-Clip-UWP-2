using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace Video_Clip2.Medias.Models
{
    public sealed class Photo : Media
    {

        //@Instance
        public static TokenDictionary<Photo> Instances = new TokenDictionary<Photo>();

        //@Override
        public override async Task ConstructSource(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            ImageProperties properties = await file.Properties.GetImagePropertiesAsync();

            uint width = properties.Width;
            uint height = properties.Height;

            const int scaledHeight = 50;
            int scaledWidth = (int)(scaledHeight * width / height);

            float scaleX = 1f * scaledWidth / width;
            float scaleY = 1f * scaledHeight / height;
            float scale = Math.Max(0.01f, Math.Max(scaleX, scaleY));

            CanvasRenderTarget thumbnail = new CanvasRenderTarget(resourceCreator, scaledWidth, scaledHeight, 96);
            using (CanvasDrawingSession drawingSession = thumbnail.CreateDrawingSession())
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream);
                drawingSession.DrawImage(new ScaleEffect
                {
                    Source = bitmap,
                    InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                    Scale = new Vector2(scale)
                });

                this.Width = width;
                this.Height = height;
                this.File = file;
                this.Bitmap = bitmap;
                this.Thumbnail = thumbnail;
            }
        }

        //@Property
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public IStorageFile File { get; private set; }
        public CanvasBitmap Bitmap { get; private set; }
        public CanvasBitmap Thumbnail { get; private set; }

    }
}