using HelpDesk.ConsumerEventSrvice.DTO;
using HelpDesk.ConsumerEventSrvice.EmailTemplateServices;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

namespace HelpDesk.ConsumerEventSrvice.Sender
{
    public class EmailSender: ISender
    {
        private readonly IEmailTemplateService emailTemplateService;

        public EmailSender(IEmailTemplateService emailTemplateService)
        {
            this.emailTemplateService = emailTemplateService;
        }

        public void Send(UserEventSubscribeDTO evnt, string subject, string messageTemplate = null)
        {
            var emailHtmlBody = emailTemplateService.GetEmailBody(evnt, messageTemplate);
            sendEmail(evnt.Email, subject, emailHtmlBody);
        }
        

        private void sendEmail(string email, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                smtpClient.Send(new MailMessage(smtpSection.From, email, subject, body));
            }
            catch (Exception ex)
            {
                //log.Error("Application error", ex);
            }

        }
    }
}
