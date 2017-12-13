using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Resources;
using MassTransit.Logging;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

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

        public void Send(UserEventSubscribeDTO msg, string subject, string messageTemplateName)
        {
            var emailHtmlBody = emailTemplateService.GetEmailBody(msg, messageTemplateName);
            sendEmail(msg.Email, subject, emailHtmlBody);
        }
        

        private void sendEmail(string email, string subject, string body)
        {
            try
            {
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var smtpClient = new SmtpClient();
                
                var msg = new MailMessage($"{Resource.AppName} <{smtpSection.From}>" , email, subject, body);
                msg.IsBodyHtml = true;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
