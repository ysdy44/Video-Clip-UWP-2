using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Video_Clip2.Medias
{
    public sealed class TokenDictionary<T> : Dictionary<string, T>
    {

        readonly Func<ICanvasResourceCreator, string, StorageFile, Task<T>> CreateFunc;

        public TokenDictionary(Func<ICanvasResourceCreator, string, StorageFile, Task<T>> createFunc)
        {
            this.CreateFunc = createFunc;
        }

        public async Task<T> CreateAsync(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            string token = StorageApplicationPermissions.FutureAccessList.Add(file);
            if (base.ContainsKey(token)) return base[token];

            T t = await this.CreateFunc(resourceCreator, token, file);
            base.Add(token, t);
            return t;
        }

        public async Task<T> CreateAsync(ICanvasResourceCreator resourceCreator, string token)
        {
            if (base.ContainsKey(token)) return base[token];
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);

            T t = await this.CreateFunc(resourceCreator, token, file);
            base.Add(token, t);
            return t;
        }

    }
}