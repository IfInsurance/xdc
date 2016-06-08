using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Nimbus.Infrastructure;
using System.Reflection;
using Nimbus.Configuration;
using Nimbus;

namespace CloudService1.EchoMessageHandler
{
    public static class NimbusConfiguration
    {
        static Bus bus;

        [NoAutomaticTrigger]
        public static async Task Startup()
        {
            var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var typeProvider = new AssemblyScanningTypeProvider(Assembly.Load("CloudService1.Public"));
            bus = new BusBuilder()
                .Configure()
                    .WithNames("EchoMessageHandler", Environment.MachineName)
                    .WithConnectionString(connectionString)
                    .WithTypesFrom(typeProvider)
                    .WithDefaultTimeout(TimeSpan.FromSeconds(10))
                    .Build();
            await bus.Start();
        }

        [NoAutomaticTrigger]
        public static async Task Stop()
        {
            await bus.Stop();
        }
    }
}
