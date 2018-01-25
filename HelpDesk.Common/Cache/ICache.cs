using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    public interface ICache
    {
        T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0);
        object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0);
        void Remove(string key);
    }
}
