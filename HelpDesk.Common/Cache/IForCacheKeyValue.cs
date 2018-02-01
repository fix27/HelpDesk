namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Если в качестве параметра для кэша используется объект не примитивного типа 
    /// </summary>
    public interface IForCacheKeyValue
    {
        string GetForCacheKeyValue();
    }
}
