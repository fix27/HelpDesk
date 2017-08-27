namespace HelpDesk.Data.Repository
{
    /// <summary>
    /// Для фиксации изменений UofW в БД
    /// </summary>
    public interface IRepository
    {
        void SaveChanges();

    }
}
