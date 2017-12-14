using System;
using System.Collections.Generic;

namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserDeedlineAppEventSubscribeDTO: UserEventSubscribeDTO
    {
        public IEnumerable<RequestDTO> Items { get; set; }
    }
}
