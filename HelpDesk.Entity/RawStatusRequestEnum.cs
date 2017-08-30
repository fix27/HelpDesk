namespace HelpDesk.Entity
{
    /// <summary>
    /// Значения Id для StatusRequest
    /// </summary>
    public enum RawStatusRequestEnum
    {
        /// <summary>
        /// Рассмотрение
        /// </summary>
        New = 1000,

        /// <summary>
        /// Дата окончания
        /// </summary>
        DateEnd = 1100,

        /// <summary>
        /// Принята
        /// </summary>
        Accepted = 2000,

        /// <summary>
        /// Отказано
        /// </summary>
        Rejected = 2100,

        /// <summary>
        /// Отказ после принятия
        /// </summary>
        RejectedAfterAccepted = 2200,

        /// <summary>
        /// Перенос
        /// </summary>
        ExtendedDeadLine = 2300,

        /// <summary>
        /// Выполнена (закрыта исполнителем)
        /// </summary>
        Closing = 2400,

        /// <summary>
        /// Перенос подтверждения
        /// </summary>
        ExtendedConfirmation = 3000,

        /// <summary>
        /// Отказано в готовности
        /// </summary>
        NotApprovedComplete = 3100,

        /// <summary>
        /// Подтверждение отказа
        /// </summary>
        ApprovedRejected = 3200,
        
        /// <summary>
        /// Подтверждение выполнения
        /// </summary>
        ApprovedComplete = 3300,
       
        /// <summary>
        /// Пассивная (в архиве, но не выполненная)
        /// </summary>
        Passive = 3400
    }
}
