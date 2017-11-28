using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    /// <summary>
    /// Email-шаблонизатор
    /// </summary>
    public interface IEmailTemplateService
    {
        string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate);
    }
}
