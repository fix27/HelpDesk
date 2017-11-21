namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestAppEvent: IAppEvent
    {
        public long RequestEventId { get; set; }
    }
}
