using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace Video_Clip2.Medias
{
    public sealed partial class Audio
    {

        //@Property
        public string Name { get; private set; }
        public string FileType { get; private set; }
        public string Token { get; private set; }

        public TimeSpan Duration { get; private set; }
        public IStorageFile File { get; private set; }

        public IMediaPlaybackSource CreateSource() => MediaSource.CreateFromStorageFile(this.File);

        public Audiotape ToAudiotape()
        {
            return new Audiotape
            {
                Name = this.Name,
                FileType = this.FileType,
                Token = this.Token,
            };
        }

    }
}