namespace HelpDesk.Entity
{
    /// <summary>
    /// Объект сотрудника обслуживаемой организации, на который он может подавать заявку
    /// </summary>
    public class EmployeeObject: BaseEntity
    {
        public virtual Employee Employee { get; set; }
        public virtual RequestObject Object { get; set; }
    }
}
