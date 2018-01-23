using System;

namespace HelpDesk.Common.Cache
{
    public interface ICache
    {
        T AddOrGetExisting<T>(object key, Func<T> get);
    }
}
