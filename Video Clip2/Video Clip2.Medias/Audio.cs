using Microsoft.Graphics.Canvas;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Video_Clip2.Medias
{
    public sealed class Audio : Media
    {

        //@Instance
        public static TokenDictionary<Audio> Tokener = new TokenDictionary<Audio>(async (ICanvasResourceCreator resourceCreator, string token, StorageFile file) =>
        {
            MusicProperties properties = await file.Properties.GetMusicPropertiesAsync();
            TimeSpan duration = properties.Duration;

            return new Audio
            {
                Duration = duration,
                File = file,

                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token,
            };
        });

        //@Property
        public TimeSpan Duration { get; private set; }
        public IStorageFile File { get; private set; }

        public IMediaPlaybackSource CreateSource() => MediaSource.CreateFromStorageFile(this.File);

    }
}