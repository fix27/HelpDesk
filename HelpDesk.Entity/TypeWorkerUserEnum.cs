using HelpDesk.Entity.Resources;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Группа пользователя
    /// </summary>
    public enum TypeWorkerUserEnum
    {
        /// <summary>
        /// Исполнитель работ по заявкам
        /// </summary>
        [Display(Name = "UserGroupEnum_Worker", ResourceType = typeof(Resource))]
        Worker = 0,

        /// <summary>
        /// Диспетчер (только диспетчеризация)
        /// </summary>
        [Display(Name = "UserGroupEnum_Dispatcher", ResourceType = typeof(Resource))]
        Dispatcher = 1,

        /// <summary>
        /// Исполнитель работ по заявкам с возможностью диспетчеризации
        /// </summary>
        [Display(Name = "UserGroupEnum_WorkerAndDispatcher", ResourceType = typeof(Resource))]
        WorkerAndDispatcher = 2
    }
}
