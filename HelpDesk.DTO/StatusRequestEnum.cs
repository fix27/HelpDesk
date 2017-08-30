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
        Unknown,

        /// <summary>
        /// Новая
        /// </summary>
        [Display(Name = "StatusRequestEnum_New", ResourceType = typeof(Resource))]
        New,

        /// <summary>
        /// Принята в работу
        /// </summary>
        [Display(Name = "StatusRequestEnum_Accepted", ResourceType = typeof(Resource))]
        Accepted,

        /// <summary>
        /// Отказано
        /// </summary>
        [Display(Name = "StatusRequestEnum_Rejected", ResourceType = typeof(Resource))]
        Rejected,

        /// <summary>
        /// Перенос срока выполнения
        /// </summary>
        [Display(Name = "StatusRequestEnum_ExtendedDeadLine", ResourceType = typeof(Resource))]
        ExtendedDeadLine,

        /// <summary>
        /// Закрыта исполнителем
        /// </summary>
        [Display(Name = "StatusRequestEnum_Closing", ResourceType = typeof(Resource))]
        Closing,

        /// <summary>
        /// Перенос подтверждения
        /// </summary>
        [Display(Name = "StatusRequestEnum_ExtendedConfirmation", ResourceType = typeof(Resource))]
        ExtendedConfirmation,

        /// <summary>
        /// Подтверждение готовности
        /// </summary>
        [Display(Name = "StatusRequestEnum_ApprovedComplete", ResourceType = typeof(Resource))]
        ApprovedComplete,

        /// <summary>
        /// Не подтверждение готовности
        /// </summary>
        [Display(Name = "StatusRequestEnum_NotApprovedComplete", ResourceType = typeof(Resource))]
        NotApprovedComplete,

        /// <summary>
        /// Пассивная (в архиве, но не выполненная)
        /// </summary>
        [Display(Name = "StatusRequestEnum_Passive", ResourceType = typeof(Resource))]
        Passive
    }
}
