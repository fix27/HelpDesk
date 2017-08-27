using System;

namespace HelpDesk.Data.Cache
{
    public interface IInMemoryCache
    {
        T AddOrGetExisting<T>(object key, Func<T> get);
    }
}
