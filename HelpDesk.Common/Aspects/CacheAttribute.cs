using HelpDesk.Common.Cache;
using System;

namespace HelpDesk.Common.Aspects
{
    /// <summary>
    /// Для пометки метода, который должен выполняться в рамках транзакции БД
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute 
    {
        public string CacheKeyTemplate { get; set; }
        public ICache Cache { get; set; }
    }
}
