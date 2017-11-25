using HelpDesk.ConsumerEventSrvice.DTO;

namespace HelpDesk.ConsumerEventSrvice.EmailTemplateServices
{
    public interface IEmailTemplateService
    {
        string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate = null);
    }
}
