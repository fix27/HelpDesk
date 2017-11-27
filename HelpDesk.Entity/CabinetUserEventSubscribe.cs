namespace HelpDesk.Entity
{
    /// <summary>
    /// Подписка пользователя личного кабинета на события заявки
    /// </summary>
    public class CabinetUserEventSubscribe : BaseEntity
    {
        /// <summary>
        /// Пользователь личного кабинета
        /// </summary>
        public CabinetUser User { get; set; }

        /// <summary>
        /// Состояние заявки
        /// </summary>
        public StatusRequest StatusRequest { get; set; }
    }
}
