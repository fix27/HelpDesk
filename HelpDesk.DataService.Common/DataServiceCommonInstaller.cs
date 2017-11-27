using HelpDesk.DataService.Common.Interface;
using Unity;

namespace HelpDesk.DataService.Common
{
    public static class DataServiceCommonInstaller
    {
        public static void Install(IUnityContainer container)
        {
            container.RegisterType<IStatusRequestMapService, StatusRequestMapService>();
        }
    }
}
