using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface ICultureService
    {
        IReadOnlyList<string> GetList();
    }
}
