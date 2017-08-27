namespace HelpDesk.Entity
{
    /// <summary>
    /// Объект заявки
    /// </summary>
    public class RequestObject : BaseEntity
    {
        /// <summary>
        /// Наименование ПО
        /// </summary>
        public string SoftName { get; set; }
        
        /// <summary>
        /// Тип объекта
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Тип оборудования
        /// </summary>
        public HardType HardType { get; set; }

        /// <summary>
        /// Модель оборудования
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Только для фильтрации
        /// </summary>
        public bool Archive { get; set; }

    }
}
