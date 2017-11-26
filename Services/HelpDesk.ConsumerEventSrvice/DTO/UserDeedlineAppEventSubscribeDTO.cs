using System;

namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserDeedlineAppEventSubscribeDTO: UserEventSubscribeDTO
    {
        public long RequestId { get; set; }
        public string RequestStatusName { get; set; }
        public DateTime DateEndPlan { get; set; }
    }
}
