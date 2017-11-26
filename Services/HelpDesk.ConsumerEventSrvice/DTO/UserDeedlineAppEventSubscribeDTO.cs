using System;
using System.Collections.Generic;

namespace HelpDesk.ConsumerEventService.DTO
{
    public class DeedlineItem
    {
        public long RequestId { get; set; }
        public string RequestStatusName { get; set; }
        public DateTime DateEndPlan { get; set; }
    }

    public class UserDeedlineAppEventSubscribeDTO: UserEventSubscribeDTO
    {
        public IEnumerable<DeedlineItem> Items { get; set; }
    }
}
