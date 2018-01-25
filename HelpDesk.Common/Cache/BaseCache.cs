namespace HelpDesk.Common.Cache
{
    public abstract class BaseCache
    {
        protected readonly int defaultExpirationSeconds;

        public BaseCache(int defaultExpirationSeconds)
        {
            this.defaultExpirationSeconds = defaultExpirationSeconds;
        }
    }
}
