using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using Microsoft.Practices.Unity;
using Quartz;
using System;

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