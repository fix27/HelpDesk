using System.Collections.Generic;

namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserDeedlineAppEventSubscribeDTO: BaseUserEventSubscribeDTO
    {
        public IEnumerable<RequestDTO> Items { get; set; }
    }
}
