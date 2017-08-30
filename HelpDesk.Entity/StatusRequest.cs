namespace HelpDesk.Entity
{
    /// <summary>
    /// Справочник состояний заявки
    /// </summary>
    public class StatusRequest: SimpleEntity
    {
        /// <summary>
        /// Цвет фона для подсветки состояния
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// Список Id состояний заявки ч/з запятую, 
        /// в которые возможен переход из текущего состояния
        /// </summary>
        public string AllowableStates { get; set; }

        /// <summary>
        /// Наименование кнопки для перехода в состояние
        /// </summary>
        public string ActionName { get; set; }
    }
}
