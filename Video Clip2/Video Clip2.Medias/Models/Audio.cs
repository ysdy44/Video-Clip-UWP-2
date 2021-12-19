using Microsoft.Graphics.Canvas;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Video_Clip2.Medias.Models
{
    public sealed class Audio : Media
    {

        //@Instance
        public static TokenDictionary<Audio> Instances = new TokenDictionary<Audio>();

        //@Override
        public override async Task ConstructSource(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            MusicProperties properties = await file.Properties.GetMusicPropertiesAsync();
            TimeSpan duration = properties.Duration;

            this.Duration = duration;
            this.File = file;
        }

        //@Property
        public TimeSpan Duration { get; private set; }
        public IStorageFile File { get; private set; }

        public IMediaPlaybackSource CreateSource() => MediaSource.CreateFromStorageFile(this.File);

    }
}