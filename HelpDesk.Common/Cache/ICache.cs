using System;

namespace HelpDesk.Common.Cache
{
    public interface ICache
    {
        T AddOrGetExisting<T>(string key, Func<T> valueFactory);
        void Remove(string key);
    }
}
