namespace HelpDesk.Entity
{
    /// <summary>
    /// Тип пользователя
    /// </summary>
    public class TypeWorkerUser: SimpleEntity
    {
        /// <summary>
        /// Список Id состояний заявки ч/з запятую, 
        /// в которые пользователю разрешено перевести заявку
        /// </summary>
        public string AllowableStates { get; set; }
    }
}
