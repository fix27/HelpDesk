namespace HelpDesk.Entity
{
    /// <summary>
    /// Объект пользователя, на который он может подавать заявку
    /// </summary>
    public class PersonalProfileObject: BaseEntity
    {
        public virtual PersonalProfile PersonalProfile { get; set; }
        public virtual RequestObject Object { get; set; }
    }
}
