using Microsoft.Azure.WebJobs;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Azure;

namespace CloudService1.EchoMessageHandler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var configuration = new BusConfiguration();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();

            configuration.UseTransport<AzureServiceBusTransport>().ConnectionStringName("AzureWebJobsServiceBus");
            configuration.UsePersistence<AzureStoragePersistence>();
            //configuration.ApplyMessageConventions();

            var startableBus = Bus.Create(configuration);

            var hostConfiguration = new JobHostConfiguration();

            var host = new JobHost(hostConfiguration);
            //host.Call(typeof(NimbusConfiguration).GetMethod("Startup"));

            using (startableBus.Start())
            {
                host.RunAndBlock();
            }
        }
    }
}
