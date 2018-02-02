namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserRequestAppEventSubscribeDTO: BaseUserEventSubscribeDTO
    {
        public RequestDTO Request { get; set; }
        public bool IsWorker { get; set; }
    }
}
