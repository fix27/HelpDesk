namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Базовый класс кэша
    /// </summary>
    public abstract class BaseCache
    {
        protected readonly int defaultExpirationSeconds;

        public BaseCache(int defaultExpirationSeconds)
        {
            this.defaultExpirationSeconds = defaultExpirationSeconds;
        }
    }
}
