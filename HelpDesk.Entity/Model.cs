namespace HelpDesk.Entity
{
    /// <summary>
    /// Модель железа
    /// </summary>
    public class Model : SimpleEntity
    {
        /// <summary>
        /// Производитель оборудования
        /// </summary>
        public Manufacturer Manufacturer { get; set; }

    }
}
