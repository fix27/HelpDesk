using System;

namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Интерфейс кэша
    /// </summary>
    public interface ICache
    {
        object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0);
        void Remove(string key);
    }
}
