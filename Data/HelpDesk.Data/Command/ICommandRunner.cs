using System.Collections.Generic;

namespace HelpDesk.Data.Command
{
    public interface ICommandRunner
    {
        IDictionary<string, object> Run(ICommand cmd);
    }
}
