namespace HelpDesk.Common
{
    /// <summary>
    /// Пейджинг
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Количество записей на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Всего записей
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Количество записей, которое вернул запрос с учетом фильтрации
        /// </summary>
        public int Count { get; set; }
    }
}
