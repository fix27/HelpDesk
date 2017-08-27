using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Событие заявки
    /// </summary>
    public class BaseRequestEvent : BaseEntity
    {
        public virtual long RequestId { get; set; }
        public virtual StatusRequest StatusRequest { get; set; }
        public virtual DateTime DateEvent { get; set; }
        public virtual DateTime DateInsert { get; set; }
        public virtual long? TypeRequestEventId { get; set; }
        public virtual string Name { get; set; }
        public virtual int OrdGroup { get; set; }

    }
}
