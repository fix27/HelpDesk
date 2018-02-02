using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    /// <summary>
    /// Email-шаблонизатор
    /// </summary>
    public interface IEmailTemplateService
    {
        string GetEmailBody(BaseUserEventSubscribeDTO message, string messageTemplate);
    }
}
