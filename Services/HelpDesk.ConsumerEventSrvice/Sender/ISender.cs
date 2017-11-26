using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.Sender
{
    /// <summary>
    /// Отправщик сообщений
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="evnt">Сообщение</param>
        /// <param name="subject">Тепа сообщения</param>
        /// <param name="messageTemplateName">Имя шаблона сообщения</param>
        void Send(UserEventSubscribeDTO msg, string subject, string messageTemplateName = null);
    }
}
