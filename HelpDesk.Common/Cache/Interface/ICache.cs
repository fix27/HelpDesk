using System;
using System.Threading.Tasks;

namespace HelpDesk.Common.Cache.Interface
{
    /// <summary>
    /// Интерфейс кэша
    /// </summary>
    public interface ICache
    {
        T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0);        
        void Remove(string key);
    }
}
