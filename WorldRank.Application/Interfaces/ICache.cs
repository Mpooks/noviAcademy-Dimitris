using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Application.Interfaces
{
    public interface ICache
    {
        bool TryGet<T>(string key, out T? value);

        void Set<T>(
            string key,
            T value,
            TimeSpan timeToLive);

        void Remove(string key);
    }
}
