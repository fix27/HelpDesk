using HelpDesk.Data.Command;
using NHibernate;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.Data.NHibernate
{
    public class CommandRunner: ICommandRunner
    {
        private readonly ISession session;
        public CommandRunner(ISession session)
        {
            this.session = session;
        }
        public IDictionary<string, object> Run(ICommand cmd)
        {
            
            IDbCommand dbCommand = session.Connection.CreateCommand();
            dbCommand.CommandType = cmd.CommandType;
            dbCommand.CommandText = cmd.CommandText;

            if (cmd.InTransaction)
            {
                dbCommand.Transaction = session.Connection.BeginTransaction(IsolationLevel.Serializable);
            }

            IEnumerable<CommandParameter> parameters = cmd.CommandParameters;
            foreach (CommandParameter p in parameters)
            {
                IDbDataParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = p.ParameterName;
                dbParameter.DbType = p.DbType;
                dbParameter.Direction = p.Direction;

                if (p.DbType == DbType.String || p.DbType == DbType.AnsiString)
                    dbParameter.Size = p.Size;

                dbParameter.Value = p.Value;

                if (p.Value == null)
                    dbParameter.Value = "";

                dbCommand.Parameters.Add(dbParameter);
            }

            int c = dbCommand.ExecuteNonQuery();
            
            if (cmd.InTransaction)
            {
                dbCommand.Transaction.Commit();
            }

            IDictionary<string, object> results = new Dictionary<string, object>();
            foreach (CommandParameter p in parameters.Where(t => t.Direction == ParameterDirection.InputOutput || t.Direction == ParameterDirection.Output || t.Direction == ParameterDirection.ReturnValue))
            {
                results[p.ParameterName] = ((IDbDataParameter)dbCommand.Parameters[p.ParameterName]).Value;
            }

            return results;
        }
    }
}
