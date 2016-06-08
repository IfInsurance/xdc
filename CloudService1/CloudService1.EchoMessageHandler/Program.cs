using Microsoft.Azure.WebJobs;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Azure;
using NServiceBus.AzureServiceBus;

namespace CloudService1.EchoMessageHandler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var configuration = new EndpointConfiguration("CloudService1.EchoMessageHandler");
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.UseTransport<AzureServiceBusTransport>()
                .ConnectionStringName("AzureWebJobsServiceBus")
                .UseTopology<EndpointOrientedTopology>()
                .ConnectivityMode(Microsoft.ServiceBus.ConnectivityMode.Https);
            configuration.UsePersistence<InMemoryPersistence>();
            
            //configuration.UsePersistence<AzureStoragePersistence>();
            //configuration.ApplyMessageConventions();

            var endpoint = Endpoint.Create(configuration).Result;
            var instance = endpoint.Start().Result;

            var hostConfiguration = new JobHostConfiguration();

            var host = new JobHost(hostConfiguration);
            host.RunAndBlock();

            instance.Stop();
        }
    }
}
