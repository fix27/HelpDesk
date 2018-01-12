using System.Collections.Generic;

namespace HelpDesk.EventBus.Common.AppEvents.Interface
{
    /// <summary>
    /// Событие "Истекает срок по заявке"
    /// </summary>
    public interface IRequestDeedlineAppEvent : IAppEvent
    {
        /// <summary>
        /// Id заявок
        /// </summary>
        IEnumerable<long> RequestIds { get; }
    }
}
