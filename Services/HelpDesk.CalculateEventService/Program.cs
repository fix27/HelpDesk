using HelpDesk.CalculateEventService.Jobs;
using HelpDesk.EventBus;
using Quartz;
using Quartz.Unity;
using System;
using System.Threading;
using Unity;
using CommonLoggingSimple = Common.Logging.Simple;
using CommonLogging = Common.Logging;

namespace HelpDesk.CalculateEventService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                IUnityContainer container = UnityConfig.GetConfiguredContainer();
                container.AddNewExtension<QuartzUnityExtension>();
                
                CommonLogging.LogManager.Adapter = new CommonLoggingSimple.ConsoleOutLoggerFactoryAdapter { Level = CommonLogging.LogLevel.Info };

                // do your other Unity registrations
                IScheduler scheduler = container.Resolve<IScheduler>();

                // and start it off
                scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<CalculateRequestDeedlineAppEventJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(2)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger);

                // some sleep to show what's happening
                Thread.Sleep(TimeSpan.FromSeconds(60));

                // and last shut down the scheduler when you are ready to close your program
                scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }

            RabbitMQBusControlManager.StopBus();
            Console.WriteLine("Press any key to close the application");
            
            Console.ReadKey();
        }
    }
}
