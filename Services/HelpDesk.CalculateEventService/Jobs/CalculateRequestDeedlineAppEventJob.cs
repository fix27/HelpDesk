using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using Quartz;
using System;
using Unity;

namespace HelpDesk.CalculateEventService.Jobs
{
    public class CalculateRequestDeedlineAppEventJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            var queue = UnityConfig.GetConfiguredContainer().Resolve<IQueue<IRequestDeedlineAppEvent>>();
            Console.WriteLine("-");
        }
    }
}