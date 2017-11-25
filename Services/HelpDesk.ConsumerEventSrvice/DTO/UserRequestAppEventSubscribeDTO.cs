using System;

namespace HelpDesk.ConsumerEventSrvice.DTO
{
    public class UserRequestAppEventSubscribeDTO: UserEventSubscribeDTO
    {
        public long RequestId { get; set; }
        public string RequestStatusName { get; set; }
        public DateTime DateEndPlan { get; set; }
    }
}
