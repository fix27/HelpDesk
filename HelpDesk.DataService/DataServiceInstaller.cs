using HelpDesk.Common.Aspects;
using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Query;
using HelpDesk.Entity;
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

			container.RegisterType<IEmployeeRequestQuery<Request>, EmployeeRequestQuery<Request>>();
			container.RegisterType<IEmployeeRequestQuery<RequestArch>, EmployeeRequestQuery<RequestArch>>();
			container.RegisterType<IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>>, AllowableObjectISQuery>();
			container.RegisterType<IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>>, AllowableObjectTypeQuery>();
			container.RegisterType<IQuery<ArchiveYearQueryParam, IEnumerable<Year>>, ArchiveYearQuery>();
			container.RegisterType<IQuery<DescriptionProblemQueryParam, IEnumerable<SimpleDTO>>, DescriptionProblemQuery>();
			container.RegisterType<IQuery<EmployeeArchiveYearQueryParam, IEnumerable<Year>>, EmployeeArchiveYearQuery>();
			container.RegisterType<IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>, EmployeeObjectQuery>();
			container.RegisterType<IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>>, RequestLastEventQuery>();
			container.RegisterType<IQuery<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>>, RequestStateCountQuery>();
			container.RegisterType<IRequestQuery<Request>, RequestQuery<Request>>();
			container.RegisterType<IRequestQuery<RequestArch>, RequestQuery<RequestArch>>();

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
