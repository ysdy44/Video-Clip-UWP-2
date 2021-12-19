using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.MediaProperties;
using Windows.Media.Playback;
using Windows.Media.Transcoding;
using Windows.Storage;

namespace Video_Clip2.Medias
{
    public sealed class Video : Media
    {

        //@Instance
        public static TokenDictionary<Video> Tokener = new TokenDictionary<Video>(async (ICanvasResourceCreator resourceCreator, string token, StorageFile file) =>
        {
            MediaClip clip = await MediaClip.CreateFromFileAsync(file);
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
        });

        //@Property
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public TimeSpan Duration { get; private set; }
        public IStorageFile File { get; private set; }
        public CanvasBitmap[] Thumbnails { get; private set; }
        public MediaComposition Composition { get; private set; }

        public IMediaPlaybackSource CreateSource() => MediaSource.CreateFromStorageFile(this.File);
        public void DrawThumbnails(CanvasDrawingSession drawingSession, double width, double position, double lenth)
        {
            double offset = 0;

            do
            {
                double seconds = 0.5 + position + offset / width * lenth;
                int index = (int)Math.Round(seconds, MidpointRounding.AwayFromZero);

                CanvasBitmap bitmap = index < this.Thumbnails.Length ? this.Thumbnails[index] : this.Thumbnails.Last();
                drawingSession.DrawImage(bitmap, (float)offset, 0);

                offset += bitmap.Size.Width;

            } while (offset < width);
        }
        public IAsyncOperation<ImageStream> FreezeFrame(TimeSpan position)
        {
            return this.Composition.GetThumbnailAsync(position, (int)this.Width, (int)this.Height, VideoFramePrecision.NearestFrame);
        }
        public async Task<PrepareTranscodeResult> ExtractMP3(IStorageFile destination)
        {
            return await new MediaTranscoder
            {
                VideoProcessingAlgorithm = MediaVideoProcessingAlgorithm.Default
            }.PrepareFileTranscodeAsync(this.File, destination, MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High));
        }

    }
}