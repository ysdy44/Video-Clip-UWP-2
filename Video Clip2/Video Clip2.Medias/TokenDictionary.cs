using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Video_Clip2.Medias
{
    public sealed class TokenDictionary<T> : Dictionary<string, T>
        where T : Media, new()
    {

        public async Task<T> CreateAsync(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (base.ContainsKey(token)) return base[token];

            T t = new T
            {
                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token
            };
            await t.ConstructSource(resourceCreator, file);

            base.Add(token, t);
            return t;
        }

        public async Task<T> CreateAsync(ICanvasResourceCreator resourceCreator, string token)
        {
            if (base.ContainsKey(token)) return base[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            T t = new T
            {
                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token
            };
            await t.ConstructSource(resourceCreator, file);

            base.Add(token, t);
            return t;
        }
        public async Task<T> CreateAsync(ICanvasResourceCreator resourceCreator, string token, StorageFile file)
        {
            T t = new T
            {
                Name = file.DisplayName,
                FileType = file.FileType,
                Token = token
            };

            await t.ConstructSource(resourceCreator, file);
            return t;
        }

    }
}