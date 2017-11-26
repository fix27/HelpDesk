using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using MassTransit.Logging;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

namespace HelpDesk.ConsumerEventService.Sender
{
    public class EmailSender: ISender
    {
        private readonly IEmailTemplateService emailTemplateService;
        private readonly ILog log;
        public EmailSender(IEmailTemplateService emailTemplateService, ILog log)
        {
            this.emailTemplateService = emailTemplateService;
            this.log = log;
        }

        public void Send(UserEventSubscribeDTO msg, string subject, string messageTemplateName = null)
        {
            var emailHtmlBody = emailTemplateService.GetEmailBody(msg, messageTemplateName);
            sendEmail(msg.Email, subject, emailHtmlBody);
        }
        

        private void sendEmail(string email, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(new MailMessage(smtpSection.From, email, subject, body));
        }
    }
}
