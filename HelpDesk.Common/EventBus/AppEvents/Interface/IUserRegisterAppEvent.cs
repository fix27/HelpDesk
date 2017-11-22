namespace HelpDesk.Common.EventBus.AppEvents.Interface
{
    public interface IUserRegisterAppEvent: IAppEvent
    {
        string Email { get; }
    }
}
