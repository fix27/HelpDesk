namespace HelpDesk.Entity
{
    public enum TypeWorkerUserEnum
    {
        /// <summary>
        /// Исполнитель
        /// </summary>
        Worker = 0,

        /// <summary>
        /// Диспетчер
        /// </summary>
        Dispatcher = 1,

        /// <summary>
        /// Исполнитель-диспетчер
        /// </summary>
        WorkerAndDispatcher = 2
    }

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

        /// <summary>
        /// Тип пользователя (взаимооднозначно соответствует Id)
        /// </summary>
        public TypeWorkerUserEnum TypeCode { get; set; }
    }
}
