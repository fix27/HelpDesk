using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Resources;
using MassTransit.Logging;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HelpDesk.ConsumerEventService.Sender
{
    /// <summary>
    /// Отправщик сообщений, отформатированных как html (на основе Razor-шаблона), на e-mail
    /// </summary>
    public class EmailSender: ISender
    {
        private readonly IEmailTemplateService emailTemplateService;
        private readonly ILog log;
        public EmailSender(IEmailTemplateService emailTemplateService, ILog log)
        {
            this.emailTemplateService = emailTemplateService;
            this.log = log;
        }

        public Task SendAsync(UserEventSubscribeDTO msg, string subject, string messageTemplateName)
        {
            var emailHtmlBody = emailTemplateService.GetEmailBody(msg, messageTemplateName);
            Task t = new Task(() => sendEmail(msg.Email, subject, emailHtmlBody));
            return t;
        }
        

        private Task sendEmail(string email, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var smtpClient = new SmtpClient();

            var msg = new MailMessage($"{Resource.AppName} <{smtpSection.From}>", email, subject, body);
            msg.IsBodyHtml = true;
            return smtpClient.SendMailAsync(msg);
        }
    }
}
