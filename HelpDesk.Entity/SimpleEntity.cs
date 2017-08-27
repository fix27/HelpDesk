namespace HelpDesk.Entity
{
    
    /// <summary>
    /// Простоя сущность - только свойство Name
    /// </summary>
    public class SimpleEntity : BaseEntity
    {
        /// <summary>
        /// Наименование (или какое-то похожее свойство)
        /// </summary>
        public string Name { get; set; }
    }
}
