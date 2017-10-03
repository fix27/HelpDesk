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
        /// Количество дней по-умолчанию на выполнение работ по объекту данного типа
        /// </summary>
        public int CountDay { get; set; }
    }
}
