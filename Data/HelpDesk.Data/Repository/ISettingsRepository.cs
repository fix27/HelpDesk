using HelpDesk.Entity;


namespace HelpDesk.Data.Repository
{
    /// <summary>
    /// Для работы с настройками системы
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Возвращает первую запись ис таблицы с настройками системы
        /// </summary>
        Settings Get();
    }
}
