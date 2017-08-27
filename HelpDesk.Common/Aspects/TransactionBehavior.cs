using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;


namespace HelpDesk.Common.Aspects
{
    /// <summary>
    /// Перехватчик выполнения метода, оборачивающий метод в транзакцию
    /// </summary>
    public class TransactionBehavior : IInterceptionBehavior
    {
        private IMethodReturn run(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, TransactionScope transaction = null)
        {
            var result = getNext()(input, getNext);

            if (result.Exception == null)
            {
                if(transaction != null)
                    transaction.Complete();
                return result;
            }
            else
                return input.CreateExceptionMethodReturn(result.Exception);
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            MethodInfo methodInfo = input.Target.GetType().GetMethods().ToList()
                .FirstOrDefault(m => m.Name == input.MethodBase.Name);
            TransactionAttribute transactionAttribute = 
                methodInfo != null? methodInfo.GetCustomAttribute<TransactionAttribute>(): null;
            

            if (transactionAttribute == null)
            {
                return run(input, getNext);
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    return run(input, getNext, transaction);
                }
            }

        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
