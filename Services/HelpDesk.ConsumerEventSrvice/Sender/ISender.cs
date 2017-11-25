using HelpDesk.ConsumerEventSrvice.DTO;

namespace HelpDesk.ConsumerEventSrvice.Sender
{
    public interface ISender
    {
        void Send(UserEventSubscribeDTO evnt, string subject, string messageTemplate = null);
    }
}
