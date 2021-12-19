﻿using Microsoft.Graphics.Canvas;
using System;
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
    public sealed partial class Video
    {

        //@Property
        public string Name { get; private set; }
        public string FileType { get; private set; }
        public string Token { get; private set; }

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

        public Videotape ToVideotape()
        {
            return new Videotape
            {
                Name = this.Name,
                FileType = this.FileType,
                Token = this.Token,
            };
        }

    }
}