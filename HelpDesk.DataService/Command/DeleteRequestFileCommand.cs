using HelpDesk.Data.Command;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.DataService.Command
{
    /// <summary>
    /// Команда: привязка загруженных в БД файлов к заявке по TempRequestKey
    /// </summary>
    public class DeleteRequestFileCommand : ICommand
    {
        
        private readonly long requestId;

        public DeleteRequestFileCommand(long requestId)
        {
            this.requestId = requestId;
        }

        public IEnumerable<CommandParameter> CommandParameters
        {
            get
            {
                IList<CommandParameter> list = new List<CommandParameter>();

                list.Add(new CommandParameter("@requestId",         DbType.Int64,   requestId));
                
                return list;
            }
        }
        public string CommandText
        {
            get
            {
                return "delete RequestFile where RequestId = @requestId";
            }
        }

        public CommandType CommandType
        {
            get
            {
                return CommandType.Text;
            }
        }

        public bool InTransaction { get { return false; } }
    }
}
