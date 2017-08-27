using HelpDesk.Data.Command;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.DataService.Command
{
    /// <summary>
    /// Команда: привязка загруженных в БД файлов к заявке по TempRequestKey
    /// </summary>
    public class UpdateRequestFileCommand : ICommand
    {
        private readonly Guid tempRequestKey;
        private readonly long requestId;

        public UpdateRequestFileCommand(Guid tempRequestKey, long requestId)
        {
            this.tempRequestKey = tempRequestKey;
            this.requestId = requestId;
        }

        public IEnumerable<CommandParameter> CommandParameters
        {
            get
            {
                IList<CommandParameter> list = new List<CommandParameter>();

                list.Add(new CommandParameter("@requestId",         DbType.Int64,   requestId));
                list.Add(new CommandParameter("@tempRequestKey",    DbType.Guid,    tempRequestKey));

                return list;
            }
        }
        public string CommandText
        {
            get
            {
                return "update RequestFile set RequestId = @requestId where tempRequestKey = @tempRequestKey";
            }
        }

        public CommandType CommandType
        {
            get
            {
                return CommandType.Text;
            }
        }

        public bool InTransaction { get { return true; } }
    }
}
