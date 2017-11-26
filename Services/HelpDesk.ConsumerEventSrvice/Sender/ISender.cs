using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.Sender
{
    public interface ISender
    {
        void Send(UserEventSubscribeDTO evnt, string subject, string messageTemplate = null);
    }
}
