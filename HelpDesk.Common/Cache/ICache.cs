using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    public interface ICache
    {
        T AddOrGetExisting<T>(string key, Func<T> valueFactory, CacheItemPolicy policy = null);
        void Remove(string key);
    }
}
