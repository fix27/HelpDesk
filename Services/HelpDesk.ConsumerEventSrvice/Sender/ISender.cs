using HelpDesk.ConsumerEventService.DTO;
using System.Threading.Tasks;

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
        /// <param name="msg">Сообщение</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="messageTemplateName">Имя шаблона сообщения</param>
        Task SendAsync(UserEventSubscribeDTO msg, string subject, string messageTemplateName);
    }
}
