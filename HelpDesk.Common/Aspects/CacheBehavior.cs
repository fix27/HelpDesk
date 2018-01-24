using HelpDesk.Common.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
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
                    var cacheImplementation = CacheInstaller.GetCache(cacheAttribute.Location.ToString());
                    if (cacheImplementation != null)
                    {
                        IList<object> methodPapameters = new List<object>();
                        IEnumerator enumerator = input.Arguments.GetEnumerator();
                        int ind = 0;
                        while (enumerator.MoveNext())
                        {
                            if (cacheAttribute.SkippedParameterIndexes == null ||
                                !cacheAttribute.SkippedParameterIndexes.Contains(ind))
                            {
                                IForCacheKeyValue forCacheKeyValue = enumerator.Current as IForCacheKeyValue;
                                if(forCacheKeyValue != null)
                                    methodPapameters.Add(forCacheKeyValue.GetForCacheKeyValue());
                                else
                                    methodPapameters.Add(enumerator.Current);
                            }
                            
                            ind++;
                        }

                        
                        if (cacheAttribute.Invalidate)
                        {
                            if (!String.IsNullOrWhiteSpace(cacheAttribute.InvalidateCacheKeyTemplates))
                            {
                                string[] invalidateCacheKeyTemplates = cacheAttribute.InvalidateCacheKeyTemplates.Split(',');
                                foreach (var t in invalidateCacheKeyTemplates)
                                    cacheImplementation.Remove(String.Format(t.Trim(), methodPapameters.ToArray()));
                            }
                            
                        }
                        else
                        {

                            string cacheKey = String.Format(cacheAttribute.CacheKeyTemplate, methodPapameters.ToArray());
                            if (cacheAttribute.AbsoluteExpiration == 0)
                                result.ReturnValue = cacheImplementation
                                    .AddOrGetExisting(cacheKey,
                                    () =>
                                    {
                                        return result.ReturnValue;
                                    });
                            else
                            {
                                result.ReturnValue = cacheImplementation
                                    .AddOrGetExisting(cacheKey,
                                    () =>
                                    {
                                        return result.ReturnValue;
                                    },
                                    new CacheItemPolicy()
                                    {
                                        AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheAttribute.AbsoluteExpiration)
                                    });
                            }
                        }                        

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
