using HelpDesk.Data.Command;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.DataService.Command
{
    /// <summary>
    /// Команда: перенос заявки вместе со всеми событиями в архив заявок
    /// </summary>
    public class TransferRequestToArchiveCommand : ICommand
    {
        
        private readonly long requestId;
        private readonly DateTime dateEndFact;
        
        public TransferRequestToArchiveCommand(long requestId, DateTime dateEndFact)
        {
            this.requestId          = requestId;
            this.dateEndFact        = dateEndFact;
        }

        public IEnumerable<CommandParameter> CommandParameters
        {
            get
            {
                IList<CommandParameter> list = new List<CommandParameter>();

                list.Add(new CommandParameter("@dateEndFact",      DbType.DateTime, dateEndFact));
                list.Add(new CommandParameter("@requestId",        DbType.Int64,    requestId));
                
                return list;
            }
        }
        public string CommandText
        {
            get
            {
                return @"begin
	                        insert into RequestArch(Id,
		                           Version,
		                           DateInsert,
		                           DateUpdate,
		                           DateEndPlan,
		                           DateEndFact,
		                           DescriptionProblem,
		                           CountCorrectionDateEndPlan,
		                           StatusId,
		                           ObjectId,
		                           EmployeeId,
		                           WorkerId,
		                           UserId)
	                        SELECT Id,
		                           Version,
		                           DateInsert,
		                           DateUpdate,
		                           DateEndPlan,
		                           @dateEndFact,
		                           DescriptionProblem,
		                           CountCorrectionDateEndPlan,
		                           StatusId,
		                           ObjectId,
		                           EmployeeId,
		                           WorkerId,
		                           UserId
	                          FROM Request where id = @requestId;

	                        insert into RequestEventArch(Id,
		                           RequestId,
		                           Note,
		                           OrdGroup,
		                           DateEvent,
		                           DateInsert,
		                           StatusRequestId,
		                           UserId)
	                        SELECT Id,
		                           RequestId,
		                           Note,
		                           OrdGroup,
		                           DateEvent,
		                           DateInsert,
		                           StatusRequestId,
		                           UserId
	                          FROM RequestEvent Where RequestId = @requestId;

	                        delete from RequestEvent Where RequestId = @requestId;
	                        delete from Request Where Id = @requestId;
                        end;";
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
