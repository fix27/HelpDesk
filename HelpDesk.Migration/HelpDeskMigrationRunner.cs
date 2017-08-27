using System.Diagnostics;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using HelpDesk.Migration.Migrations;
using System;
using FluentMigrator.Runner.Processors.SqlServer;

namespace HelpDesk.Migration
{
    public class HelpDeskMigrationRunner : IHelpDeskMigrationRunner
    {
        private readonly string connectionString;
               

        public HelpDeskMigrationRunner(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private static void WriteDebugMessage(string message)
        {
            if (!String.IsNullOrEmpty(message))
                Debug.WriteLine(string.Format("MIGRATION: {0}", message));
        }

        private IAnnouncer CreateAnnouncer()
        {
            if (Debugger.IsAttached)
                return new TextWriterAnnouncer(WriteDebugMessage)
                {
                    ShowElapsedTime = true,
                    ShowSql = true
                };

            return new NullAnnouncer();
        }
        
        public void Update()
        {
            

            IAnnouncer announcer = CreateAnnouncer();
            IRunnerContext migrationContext = new RunnerContext(announcer);
            ProcessorOptions options = new ProcessorOptions { PreviewOnly = false, Timeout = 200 };
            IMigrationProcessorFactory factory = new SqlServer2008ProcessorFactory();
            Assembly migrationContainer = typeof(InitialDeployment).Assembly;

            using (IMigrationProcessor processor = factory.Create(connectionString, announcer, options))
            {
                IMigrationRunner runner = new MigrationRunner(migrationContainer, migrationContext, processor);
                runner.MigrateUp();
            }
        }
    }
}