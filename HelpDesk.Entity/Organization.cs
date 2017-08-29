namespace HelpDesk.Entity
{
    /// <summary>
    /// Обслуживаемая организация/подразделение/отдел
    /// </summary>
    public class Organization : SimpleEntity
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Для организации иерархии
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// Есть ли подчиненные узлы
        /// </summary>
        public bool HasChild { get; set; }

        /// <summary>
        /// Только для фильтрации
        /// </summary>
        public bool Archive { get; set; }

    }
}
