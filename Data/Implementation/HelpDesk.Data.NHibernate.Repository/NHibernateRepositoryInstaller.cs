using Microsoft.Practices.Unity;
using HelpDesk.Entity;
using HelpDesk.Data.Repository;

namespace HelpDesk.Data.NHibernate.Repository
{
    public static class NHibernateRepositoryInstaller
    {
        public static void Install(IUnityContainer container)
        {
            
            //регистрация репозиториев
            container.RegisterType<IRepository, BaseRepository>();
            container.RegisterType<IBaseRepository<AccessWorkerUser>, BaseRepository<AccessWorkerUser>>();
            container.RegisterType<IBaseRepository<WorkerUser>, BaseRepository<WorkerUser>>();
            container.RegisterType<IBaseRepository<EmployeeObject>, BaseRepository<EmployeeObject>>();
            container.RegisterType<IBaseRepository<RequestFile>, BaseRepository<RequestFile>>();
            
            container.RegisterType<IBaseRepository<StatusRequest>, BaseRepository<StatusRequest>>();

            container.RegisterType<IBaseRepository<RequestObject>, BaseRepository<RequestObject>>();
            container.RegisterType<IBaseRepository<ObjectType>, BaseRepository<ObjectType>>();

            container.RegisterType<IBaseRepository<HardType>, BaseRepository<HardType>>();
            container.RegisterType<IBaseRepository<Model>, BaseRepository<Model>>();
            container.RegisterType<IBaseRepository<Manufacturer>, BaseRepository<Manufacturer>>();

            container.RegisterType<IBaseRepository<Request>, BaseRepository<Request>>();
            container.RegisterType<IBaseRepository<RequestArch>, BaseRepository<RequestArch>>();

            container.RegisterType<IBaseRepository<RequestEvent>, BaseRepository<RequestEvent>>();
            container.RegisterType<IBaseRepository<RequestEventArch>, BaseRepository<RequestEventArch>>();

            container.RegisterType<IBaseRepository<Worker>, BaseRepository<Worker>>();
            container.RegisterType<IBaseRepository<CabinetUser>, BaseRepository<CabinetUser>>();
            container.RegisterType<IBaseRepository<Post>, BaseRepository<Post>>();
            container.RegisterType<IBaseRepository<OrganizationObjectTypeWorker>, BaseRepository<OrganizationObjectTypeWorker>>();
            
            container.RegisterType<IBaseRepository<Organization>, BaseRepository<Organization>>();
            container.RegisterType<IBaseRepository<Employee>, BaseRepository<Employee>>();
            container.RegisterType<ISettingsRepository, SettingsRepository>("SettingsRepository");
            
        }
        
    }
}
