﻿using HelpDesk.CalculateEventService.Jobs;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using Microsoft.Practices.Unity;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading;

namespace HelpDesk.CalculateEventService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                UnityConfig.RegisterTypes(container);
                //Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };

                // Grab the Scheduler instance from the Factory 
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

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
                Thread.Sleep(TimeSpan.FromSeconds(6));

                // and last shut down the scheduler when you are ready to close your program
                scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }

            Console.WriteLine("Press any key to close the application");
            UnityConfig.GetConfiguredContainer().Resolve<IQueue<IRequestDeedlineAppEvent>>().Stop();
            Console.ReadKey();
        }
    }
}
