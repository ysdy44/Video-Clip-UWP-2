using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace Video_Clip2.Medias
{
    public sealed partial class Photo
    {

        //@Instance
        public static IDictionary<string, Photo> Instances { get; } = new Dictionary<string, Photo>();

        //@Static
        public static async Task<Photo> CreatePhotoAsync(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (Photo.Instances.ContainsKey(token)) return Photo.Instances[token];

            Photo photo = await Photo.CreatePhotoAsync(resourceCreator, token, file);
            Photo.Instances.Add(token, photo);
            return photo;

        }
        public static async Task<Photo> CreatePhotoAsync(ICanvasResourceCreator resourceCreator, string token)
        {
            if (Photo.Instances.ContainsKey(token)) return Photo.Instances[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            Photo photo = await Photo.CreatePhotoAsync(resourceCreator, token, file);
            Photo.Instances.Add(token, photo);
            return photo;
        }

        private static async Task<Photo> CreatePhotoAsync(ICanvasResourceCreator resourceCreator, string token, StorageFile file)
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

                return new Photo
                {
                    Width = width,
                    Height = height,
                    File = file,
                    Bitmap = bitmap,
                    Thumbnail = thumbnail,

                    Name = file.DisplayName,
                    FileType = file.FileType,
                    Token = token,
                };
            }
        }

    }
}