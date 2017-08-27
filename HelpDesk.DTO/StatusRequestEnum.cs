using HelpDesk.DTO.Resources;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Состояние заявки - то, которые видит пользователь в UI. Представляет собой
    /// фактор множество для состояний из таблицы StatusRequest
    /// </summary>
    public enum StatusRequestEnum
    {
        /// <summary>
        /// Для ненастроенного отображения состояний
        /// </summary>
        [Display(Name = "StatusRequestEnum_Unknown", ResourceType = typeof(Resource))]
        Unknown = 0,

        /// <summary>
        /// Новая
        /// </summary>
        [Display(Name = "StatusRequestEnum_New", ResourceType = typeof(Resource))]
        New = 100,

        /// <summary>
        /// Принята в работу
        /// </summary>
        [Display(Name = "StatusRequestEnum_Accepted", ResourceType = typeof(Resource))]
        Accepted = 110,

        /// <summary>
        /// Отказано
        /// </summary>
        [Display(Name = "StatusRequestEnum_Rejected", ResourceType = typeof(Resource))]
        Rejected = 120,

        /// <summary>
        /// Перенос срока выполнения
        /// </summary>
        [Display(Name = "StatusRequestEnum_ExtendedDeadLine", ResourceType = typeof(Resource))]
        ExtendedDeadLine = 130,

        /// <summary>
        /// Закрыта исполнителем
        /// </summary>
        [Display(Name = "StatusRequestEnum_Closing", ResourceType = typeof(Resource))]
        Closing = 140,

        /// <summary>
        /// Перенос подтверждения готовности
        /// </summary>
        [Display(Name = "StatusRequestEnum_ExtendedConfirmation", ResourceType = typeof(Resource))]
        ExtendedConfirmation = 150,

        /// <summary>
        /// Подтверждение готовности
        /// </summary>
        [Display(Name = "StatusRequestEnum_ApprovedComplete", ResourceType = typeof(Resource))]
        ApprovedComplete = 160,

        /// <summary>
        /// Не подтверждение готовности
        /// </summary>
        [Display(Name = "StatusRequestEnum_NotApprovedComplete", ResourceType = typeof(Resource))]
        NotApprovedComplete = 170,

        /// <summary>
        /// Пассивная (в архиве, но не выполненная)
        /// </summary>
        [Display(Name = "StatusRequestEnum_Passive", ResourceType = typeof(Resource))]
        Passive = 180
    }
}
