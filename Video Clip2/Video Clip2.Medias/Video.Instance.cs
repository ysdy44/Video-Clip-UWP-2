using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Video_Clip2.Medias
{
    public sealed partial class Video
    {

        //@Instance
        public static readonly Dictionary<string, Video> Instances = new Dictionary<string, Video>();

        //@Static
        public static async Task<Video> CreateVideoAsync(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (Video.Instances.ContainsKey(token)) return Video.Instances[token];

            Video video = await Video.CreateVideoAsync(resourceCreator, token, file);
            Video.Instances.Add(token, video);
            return video;
        }

        public static async Task<Video> CreateVideoAsync(ICanvasResourceCreator resourceCreator, string token)
        {
            if (Video.Instances.ContainsKey(token)) return Video.Instances[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            Video video = await Video.CreateVideoAsync(resourceCreator, token, file);
            Video.Instances.Add(token, video);
            return video;
        }

        private static async Task<Video> CreateVideoAsync(ICanvasResourceCreator resourceCreator, string token, StorageFile file)
        {
            Windows.Media.Editing.MediaClip clip = await Windows.Media.Editing.MediaClip.CreateFromFileAsync(file);
            MediaComposition composition = new MediaComposition { Clips = { clip } };
            VideoEncodingProperties properties = clip.GetVideoEncodingProperties();

            uint width = properties.Width;
            uint height = properties.Height;
            const int scaledHeight = 50;
            int scaledWidth = (int)(scaledHeight * width / height);

            TimeSpan duration = clip.OriginalDuration;
            IEnumerable<TimeSpan> points = Enumerable.Range(0, 1 + (int)Math.Floor(duration.TotalSeconds)).Select(c => TimeSpan.FromSeconds(c));
            IReadOnlyList<ImageStream> thumbnails = await composition.GetThumbnailsAsync(points, scaledWidth, scaledHeight, VideoFramePrecision.NearestFrame);

            CanvasBitmap[] thumbnails2 = new CanvasBitmap[thumbnails.Count];
            for (int i = 0; i < thumbnails.Count; i++)
            {
                thumbnails2[i] = await CanvasBitmap.LoadAsync(resourceCreator, thumbnails[i]);
            }

            return new Video
            {
                Width = width,
                Height = height,
                Duration = duration,
                File = file,
                Thumbnails = thumbnails2,
                Composition = composition,

                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token,
            };
        }

    }
}