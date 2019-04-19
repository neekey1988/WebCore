using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Common.Cache
{
    public interface ICache
    {
        bool Enable { get; set; }
        double Expiration { get; set; }

        void Add(string key,object value, double? seconds);
        void Add<T>(string key,T value, double? seconds);
        object GetValue(string key);
        T GetValue<T>(string key);

        bool TryGetValue(string key, out object value);
        bool TryGetValue<T>(string key, out T value);

        void Remove(string key);
    }
}
