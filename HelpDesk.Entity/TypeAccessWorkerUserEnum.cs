using HelpDesk.Entity.Resources;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Тип прав исполнителя/диспетчера
    /// </summary>
    public enum TypeAccessWorkerUserEnum
    {
        /// <summary>
        /// Права на тип объекта
        /// </summary>
        [Display(Name = "TypeAccessWorkerUserEnum_ObjectType", ResourceType = typeof(Resource))]
        ObjectType = 0,

        /// <summary>
        /// Права на исполнителя
        /// </summary>
        [Display(Name = "TypeAccessWorkerUserEnum_Worker", ResourceType = typeof(Resource))]
        Worker = 1,

        /// <summary>
        /// Права на ПО
        /// </summary>
        [Display(Name = "TypeAccessWorkerUserEnum_Object", ResourceType = typeof(Resource))]
        Object = 2,

        /// <summary>
        /// Права на адрес обслуживаемой организации
        /// </summary>
        [Display(Name = "TypeAccessWorkerUserEnum_OrganizationAddress", ResourceType = typeof(Resource))]
        OrganizationAddress = 3,

        /// <summary>
        /// Права на обслуживаемую организацию
        /// </summary>
        [Display(Name = "TypeAccessWorkerUserEnum_Organization", ResourceType = typeof(Resource))]
        Organization = 4
    }
}
