using Microsoft.Azure.WebJobs;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.AzureServiceBus;
using System.Threading.Tasks;
using NServiceBus.AzureServiceBus.Addressing;

namespace CloudService1.EchoMessageHandler
{
    class Program
    {
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

            var endpoint = await Endpoint.Create(configuration);
            instance = await endpoint.Start();
        }

        [NoAutomaticTrigger]
        public static async Task Terminate()
        {
            await instance.Stop();
        }
    }
}
