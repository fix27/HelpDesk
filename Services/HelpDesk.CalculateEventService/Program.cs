using HelpDesk.CalculateEventJob.Jobs;
using Quartz;
using Quartz.Unity;
using System;
using Unity;
using CommonLoggingSimple = Common.Logging.Simple;
using CommonLogging = Common.Logging;
using System.Configuration;

namespace HelpDesk.CalculateEventJob
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                int intervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInMinutes"]);

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
                        .WithIntervalInMinutes(intervalInMinutes)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger);

                //Thread.Sleep(1000);
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
            
        }
    }
}
