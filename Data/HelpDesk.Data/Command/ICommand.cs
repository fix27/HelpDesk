using System.Collections.Generic;
using System.Data;

namespace HelpDesk.Data.Command
{
    public class CommandParameter
    {
        /// <summary>
        /// Наименование параметра
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Тип в БД
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// Размер (для varchar2)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public object Value { get; set; }

        public CommandParameter(string parameterName, DbType dbType, object val)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = val;
            Direction = ParameterDirection.Input;
        }

        public CommandParameter(string parameterName, DbType dbType, object val, int size)
            :this(parameterName, dbType, val)
        {
            
            Size = size;
        }

        public CommandParameter(string parameterName, DbType dbType, object val, ParameterDirection direction)
            : this(parameterName, dbType, val)
        {
            Direction = direction;
        }

        public CommandParameter(string parameterName, DbType dbType, object val, int size, ParameterDirection direction)
            : this(parameterName, dbType, val, size)
        {
            Direction = direction;
        }
    }

    public interface ICommand
    {
        IEnumerable<CommandParameter> CommandParameters { get; }
        string CommandText { get; }
        CommandType CommandType { get; }

        bool InTransaction { get; }
    }
}
