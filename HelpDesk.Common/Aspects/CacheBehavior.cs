using HelpDesk.Common.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace HelpDesk.Common.Aspects
{
    /// <summary>
    /// Перехватчик выполнения метода, оборачивающий метод в запрос к кэшу
    /// </summary>
    public class CacheBehavior : IInterceptionBehavior
    {
        private IMethodReturn run(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, CacheAttribute cacheAttribute)
        {
            var result = getNext()(input, getNext);
            if (result.Exception == null)
            {
                if (cacheAttribute != null)
                {
                    var cacheImplementation = CacheInstaller.GetCache(cacheAttribute.TypeCache.ToString());
                    if (cacheImplementation != null)
                    {
                        IList<object> methodPapameters = new List<object>();
                        IEnumerator enumerator = input.Arguments.GetEnumerator();
                        while (enumerator.MoveNext())
                            methodPapameters.Add(enumerator.Current);

                        string cacheKey = String.Format(cacheAttribute.CacheKeyTemplate, methodPapameters.ToArray());
                        result.ReturnValue = cacheImplementation
                            .AddOrGetExisting(cacheKey,
                            () =>
                            {
                                return result.ReturnValue;
                            });

                        return result;
                    }

                    return result;

                }
                else
                    return result;
            }
            else
                return input.CreateExceptionMethodReturn(result.Exception);
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            MethodInfo methodInfo = input.Target.GetType().GetMethods().ToList()
                .FirstOrDefault(m => m.Name == input.MethodBase.Name);
            CacheAttribute cacheAttribute = 
                methodInfo != null? methodInfo.GetCustomAttribute<CacheAttribute>(): null;


            return run(input, getNext, cacheAttribute);

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
