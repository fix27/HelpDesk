namespace HelpDesk.DataService.DTO
{
    /// <summary>
    /// Год, в котором есть заявки. Используется на форме журнала заявок в режиме "Архив"
    /// </summary>
    public class Year
    {
        /// <summary>
        /// Значение года (0 для "Все года")
        /// </summary>
        public int Ord { get; set; }

        /// <summary>
        /// Наименование (может быть "Все года")
        /// </summary>
        public string Name { get; set; }
    }
}
