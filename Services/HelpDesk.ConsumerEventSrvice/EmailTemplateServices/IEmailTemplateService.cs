using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    public interface IEmailTemplateService
    {
        string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate = null);
    }
}
