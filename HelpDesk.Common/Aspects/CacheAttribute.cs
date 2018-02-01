using HelpDesk.Common.Cache;
using System;

namespace HelpDesk.Common.Aspects
{
    /// <summary>
    /// Для пометки метода, результаты работы которого должны кэшироваться
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute 
    {
        public string CacheKeyTemplate { get; set; }
        public CacheLocation? Location { get; set; }
        public bool Invalidate { get; set; }
        public string InvalidateCacheKeyTemplates { get; set; }

        public int[] SkippedParameterIndexes { get; set; }

        public int ExpirationSeconds { get; set; }
    }
}
