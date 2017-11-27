using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.EventBus.Common.Interface
{
    /// <summary>
    /// Очередь сообщений некоторого типа (физически для всех типов сообщений используется одна очередь)
    /// </summary>
    /// <typeparam name="T">Тип сообщения</typeparam>
    public interface IQueue<T>
        where T: class, IAppEvent 
    {
        void Push(T evnt);
    }
        
}
