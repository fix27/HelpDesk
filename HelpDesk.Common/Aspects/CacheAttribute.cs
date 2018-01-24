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
        public CacheAttribute()
        {
            Location = CacheLocation.InMemory;           
        }
        public string CacheKeyTemplate { get; set; }
        public CacheLocation Location { get; set; }
        public bool Invalidate { get; set; }
        public string InvalidateCacheKeyTemplates { get; set; }

        public int[] SkippedParameterIndexes { get; set; }

        public int AbsoluteExpiration { get; set; }
    }
}
