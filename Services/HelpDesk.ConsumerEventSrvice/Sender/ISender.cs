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
        Task SendAsync(BaseUserEventSubscribeDTO msg);
    }
}
