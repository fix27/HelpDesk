using HelpDesk.Common.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.RegistrationByConvention;

namespace HelpDesk.DataService
{
    public static class DataServiceInstaller
    {
        public static void Install(IUnityContainer container)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            
            container.AddNewExtension<Interception>();
            Assembly dataService = assemblies.
                SingleOrDefault(assembly => assembly.GetName().Name == "HelpDesk.DataService");
            foreach (Type t in AllClasses.FromAssemblies(dataService)
                .Where(t => t.Name.EndsWith("Service") && t.Name != "BaseService"))
            {
                Type interfaceType = dataService.GetType("HelpDesk.DataService.Interface.I" + t.Name);

                IList<MethodInfo> mInfo = t.GetType().GetMethods().ToList();

                Attribute transactional = Attribute.GetCustomAttribute(t, typeof(TransactionAttribute));
                Attribute cached = Attribute.GetCustomAttribute(t, typeof(CacheAttribute));

                if (transactional != null && cached != null)
                {
                    container.RegisterType(interfaceType, t,
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<TransactionBehavior>(),
                        new InterceptionBehavior<CacheBehavior>()
                    );
                }
                                
                else if (transactional != null)
                {
                    container.RegisterType(interfaceType, t,
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<TransactionBehavior>()
                    );
                }
                else if (cached != null)
                {
                    container.RegisterType(interfaceType, t,
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<CacheBehavior>()
                    );
                }
                else
                    container.RegisterType(interfaceType, t);

            }
            
                        
        }
    }
}
