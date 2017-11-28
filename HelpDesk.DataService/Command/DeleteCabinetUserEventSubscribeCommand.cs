using HelpDesk.Data.Command;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.DataService.Command
{
    /// <summary>
    /// Команда: удаление подписки на события заявки пользователя личного кабинета
    /// </summary>
    public class DeleteCabinetUserEventSubscribeCommand : ICommand
    {
        
        private readonly long userId;
        private readonly IEnumerable<long> statusRequestIds;

        public DeleteCabinetUserEventSubscribeCommand(long userId, IEnumerable<long> statusRequestIds)
        {
            this.userId = userId;
            this.statusRequestIds = statusRequestIds;
        }

        public IEnumerable<CommandParameter> CommandParameters
        {
            get
            {
                IList<CommandParameter> list = new List<CommandParameter>();

                list.Add(new CommandParameter("@userId", DbType.Int64, userId));
                
                return list;
            }
        }
        public string CommandText
        {
            get
            {
                return $"delete CabinetUserEventSubscribe where UserId = @userId and StatusRequestId in ({String.Join(",",statusRequestIds)})";
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
