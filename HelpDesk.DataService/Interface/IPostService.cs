using HelpDesk.Entity;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IPostService
    {
        IEnumerable<Post> GetList(string name);
                
    }
}
