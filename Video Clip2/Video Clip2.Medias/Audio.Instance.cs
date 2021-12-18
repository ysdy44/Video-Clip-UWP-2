using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;

namespace Video_Clip2.Medias
{
    public sealed partial class Audio
    {

        //@Instance
        public static IDictionary<string, Audio> Instances { get; } = new Dictionary<string, Audio>();

        //@Static
        public static async Task<Audio> CreateAudioAsync(StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (Audio.Instances.ContainsKey(token)) return Audio.Instances[token];

            Audio audio = await Audio.CreateAudioAsync(token, file);
            Audio.Instances.Add(token, audio);
            return audio;

        }
        public static async Task<Audio> CreateAudioAsync(string token)
        {
            if (Audio.Instances.ContainsKey(token)) return Audio.Instances[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            Audio audio = await Audio.CreateAudioAsync(token, file);
            Audio.Instances.Add(token, audio);
            return audio;
        }

        private static async Task<Audio> CreateAudioAsync(string token, StorageFile file)
        {
            MusicProperties poperties = await file.Properties.GetMusicPropertiesAsync();
            TimeSpan duration = poperties.Duration;

            return new Audio
            {
                Duration = duration,
                File = file,

                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token,
            };
        }

    }
}