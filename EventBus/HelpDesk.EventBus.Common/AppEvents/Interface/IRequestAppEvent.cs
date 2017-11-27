namespace HelpDesk.EventBus.Common.AppEvents.Interface
{
    /// <summary>
    /// Событие "Изменилось состояние заявки"
    /// </summary>
    public interface IRequestAppEvent : IAppEvent
    {
        /// <summary>
        /// Id события заявки из RequestEvent
        /// </summary>
        long RequestEventId { get; }
    }
}
