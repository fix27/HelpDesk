using System;

namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserRequestAppEventSubscribeDTO: UserEventSubscribeDTO
    {
        public RequestDTO Request { get; set; }
    }
}
