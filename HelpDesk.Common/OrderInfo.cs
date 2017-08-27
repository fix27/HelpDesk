namespace HelpDesk.Common
{
    /// <summary>
    /// Информация о сортировке
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// Наименование свойстваа, по которому производится сортировка
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Направление сортировки (true - прямая)
        /// </summary>
        public bool Asc { get; set; }
    }
}
