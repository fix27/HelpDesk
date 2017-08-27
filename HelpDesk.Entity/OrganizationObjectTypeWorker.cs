namespace HelpDesk.Entity
{
    /// <summary>
    /// Типы работ и исполнители по организациям
    /// </summary>
    public class OrganizationObjectTypeWorker : BaseEntity
    {
        /// <summary>
        /// Организация
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Тип работ
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public Worker Worker { get; set; }

        

    }
}
