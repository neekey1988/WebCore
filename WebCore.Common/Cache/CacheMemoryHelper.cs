using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace WebCore.Common.Cache
{
    public class CacheMemoryHelper : ICache
    {
        private static readonly IMemoryCache memoryCache = new MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
        private static readonly object lock_MemoryCacheHelper_w = new object();

        public bool Enable { get; set; }
        public double Expiration { get; set; }

        public void Add(string key, object value, double? seconds=null)
        {
            seconds = seconds ?? Expiration;
            lock (lock_MemoryCacheHelper_w)
            {
                using (var cacheEntry = memoryCache.CreateEntry(key))
                {
                    cacheEntry.Value = value;
                    cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(seconds.Value);
                }
            }
        }

        public void Add<T>(string key, T value,double? seconds=null)
        {
            seconds = seconds ?? Expiration;
            lock (lock_MemoryCacheHelper_w)
            {
                using (var cacheEntry = memoryCache.CreateEntry(key))
                {
                    cacheEntry.Value = value;
                    cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(seconds.Value);
                }
            }

        }


        public object GetValue(string key)=> memoryCache.Get(key);

        public T GetValue<T>(string key)=> memoryCache.Get<T>(key);
        public bool TryGetValue(string key,out object value) => memoryCache.TryGetValue(key,out value);
        public bool TryGetValue<T>(string key, out T value) => memoryCache.TryGetValue<T>(key, out value);

        public void Remove(string key) => memoryCache.Remove(key);
         
    }
}
