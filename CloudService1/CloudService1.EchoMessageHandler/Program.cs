using Microsoft.Azure.WebJobs;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Azure;
using NServiceBus.AzureServiceBus;
using System.Threading.Tasks;
using NServiceBus.AzureServiceBus.Addressing;

namespace CloudService1.EchoMessageHandler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var hostConfiguration = new JobHostConfiguration();
            var host = new JobHost(hostConfiguration);
            host.Call(typeof(NServiceBusConfiguration).GetMethod(nameof(NServiceBusConfiguration.Initialize)));
            host.RunAndBlock();
            host.Call(typeof(NServiceBusConfiguration).GetMethod(nameof(NServiceBusConfiguration.Terminate)));
        }
    }

    public static class NServiceBusConfiguration
    {
        private static IEndpointInstance instance;

        [NoAutomaticTrigger]
        public static async Task Initialize()
        {
            var configuration = new EndpointConfiguration("CloudService1.EchoMessageHandler");
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            var transport = configuration.UseTransport<AzureServiceBusTransport>()
                .ConnectionStringName("AzureWebJobsServiceBus")
                .UseTopology<EndpointOrientedTopology>()
                .RegisterPublisherForType("CloudService2.ColorMessageHandler", typeof(CloudService2.Public.Events.ColorNameToRgbTranslationComplete));
            transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
            transport.Sanitization().UseStrategy<EndpointOrientedTopologySanitization>();
            transport.ConnectivityMode(Microsoft.ServiceBus.ConnectivityMode.Https);
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.EnableInstallers();
            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Public.Events"));
            //var routing = configuration.UnicastRouting();
            //routing.AddPublisher("CloudService1.EchoMessageHandler", typeof(CloudService2.Public.Commands.TranslateColorNameToRgb));
            //routing.AddPublisher("CloudService2.ColorMessageHandler", typeof(CloudService2.Public.Events.ColorNameToRgbTranslationComplete));

            //configuration.UsePersistence<AzureStoragePersistence>();
            //configuration.ApplyMessageConventions();

            var endpoint = await Endpoint.Create(configuration);
            instance = await endpoint.Start();
            //await instance.Send<CloudService2.Public.Commands.TranslateColorNameToRgb>("CloudService2.ColorMessageHandler", t => {
            //    t.ColorName = "Magenta";
            //    t.CommandId = System.Guid.NewGuid();
            //});
        }

        [NoAutomaticTrigger]
        public static async Task Terminate()
        {
            await instance.Stop();
        }
    }
}
