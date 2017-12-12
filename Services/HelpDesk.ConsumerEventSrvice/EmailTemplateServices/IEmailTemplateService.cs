using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    /// <summary>
    /// Email-шаблонизатор
    /// </summary>
    public interface IEmailTemplateService
    {
        string GetEmailBody(UserEventSubscribeDTO message, string messageTemplate);
    }
}
