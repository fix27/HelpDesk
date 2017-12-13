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

        /// <summary>
        /// Заявка перенесена в архив, или событие произошло с архивной заявкой
        /// </summary>
        bool Archive { get; }
    }
}
