namespace HelpDesk.Entity
{
    /// <summary>
    /// Подписка ползователя Исполнителя на события заявки
    /// </summary>
    public class WorkerUserEventSubscribe: BaseEntity
    {
        /// <summary>
        /// Пользователь Исполнителя
        /// </summary>
        public WorkerUser User { get; set; }

        /// <summary>
        /// Состояние заявки
        /// </summary>
        public virtual StatusRequest StatusRequest { get; set; }
    }
}
