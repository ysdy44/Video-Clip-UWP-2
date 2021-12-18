using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;

namespace Video_Clip2.Clips.ClipManagers
{
    public sealed class AudioClipManager
    {

        //@Instance
        private static readonly IDictionary<string, AudioClipManager> Instance = new Dictionary<string, AudioClipManager>();
        public static IEnumerable<string> Tokens => AudioClipManager.Instance.Keys;
        public static void Clear() => AudioClipManager.Instance.Clear();
        public static async Task<AudioClipManager> Add(StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (AudioClipManager.Instance.ContainsKey(token)) return AudioClipManager.Instance[token];

            AudioClipManager audio = await AudioClipManager.LoadAsync(token, file);
            AudioClipManager.Instance.Add(token, audio);
            return audio;
        }
        public static async Task<AudioClipManager> Add(string token)
        {
            if (AudioClipManager.Instance.ContainsKey(token)) return AudioClipManager.Instance[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            AudioClipManager audio = await AudioClipManager.LoadAsync(token, file);
            AudioClipManager.Instance.Add(token, audio);
            return audio;
        }


        //@Property
        public string Token { get; private set; }
        public TimeSpan Duration { get; private set; }
        public IStorageFile File { get; private set; }

        public IMediaPlaybackSource CreateSource() => MediaSource.CreateFromStorageFile(this.File);


        //@Static
        private static async Task<AudioClipManager> LoadAsync(string token, StorageFile file)
        {
            MusicProperties poperties = await file.Properties.GetMusicPropertiesAsync();
            TimeSpan duration = poperties.Duration;

            return new AudioClipManager
            {
                Token = token,
                Duration = duration,
                File = file,
            };
        }

    }
}