namespace HelpDesk.Entity
{
    /// <summary>
    /// Тип объекта
    /// </summary>
    public class ObjectType : SimpleEntity
    {
        /// <summary>
        /// true - тип объекта соответствует ПО
        /// </summary>
        public bool Soft { get; set; }

        /// <summary>
        /// Только для фильтрации
        /// </summary>
        public bool Archive { get; set; }

        /// <summary>
        /// Количество рабочих часов по-умолчанию на выполнение работ по объекту данного типа
        /// </summary>
        public int CountHour { get; set; }

        /// <summary>
        /// Количество рабочих часов, по истечению которых считается, 
        /// что у заявки данного типа истекает срок выполнения
        /// </summary>
        public int DeadlineHour { get; set; }
    }
}
