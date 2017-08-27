using System;

namespace HelpDesk.Common.Aspects
{
    /// <summary>
    /// Для пометки метода, который должен выполняться в рамках транзакции БД
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TransactionAttribute : Attribute 
    {

    }
}
