namespace HelpDesk.Entity
{
    /// <summary>
    /// Права исполнителя/диспетчера. Используется только одно свойство ObjectType, Worker, 
    /// Object, OrganizationAddress или Organization в зависимости от Type 
    /// </summary>
    public class AccessWorkerUser : BaseEntity
    {
        
        public WorkerUser User { get; set; }

        
        public TypeAccessWorkerUserEnum Type { get; set; }

        
        public ObjectType ObjectType { get; set; }

        
        public Worker Worker { get; set; }

        
        public RequestObject Object { get; set; }

        /// <summary>
        /// Адрес обслуживаемой организации 
        /// </summary>
        public string OrganizationAddress { get; set; }

        
        public Organization Organization { get; set; }
    }
}
