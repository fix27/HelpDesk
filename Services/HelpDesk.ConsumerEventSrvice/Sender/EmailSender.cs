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

        public Task SendAsync(BaseUserEventSubscribeDTO msg)
        {
            string subject = null; 
            string messageTemplateName = null;

            var v1 = msg as UserDeedlineAppEventSubscribeDTO;
            if (v1 != null)
            {
                subject = Resource.Subject_RequestDeedlineAppEventConsumer;
                messageTemplateName = "RequestDeedlineAppEvent";
            }

            var v2 = msg as UserDeedlineAppEventSubscribeDTO;
            if (v2 != null)
            {
                subject = Resource.Subject_UserPasswordRecoveryAppEventConsumer;
                messageTemplateName = "UserPasswordRecoveryAppEvent";
            }

            var v3 = msg as UserRequestAppEventSubscribeDTO;
            if (v3 != null)
            {
                if (v3.IsWorker)
                {
                    subject = String.Format(Resource.Subject_RequestAppEventConsumer, v3.Request.Id, v3.Request.StatusName);
                    messageTemplateName = "RequestAppEventWorker";
                }
                else
                {
                    subject = String.Format(Resource.Subject_RequestAppEventConsumer, v3.Request.Id, v3.Request.StatusName);
                    messageTemplateName = "RequestAppEventCabinet";
                }
            }

            var v4 = msg as UserEventSubscribeDTO;
            if (v4 != null)
            {
                subject = String.Format(Resource.Subject_UserRegisterAppEventConsumer, Resource.AppName);
                messageTemplateName = "UserRegisterAppEvent";
            }

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
