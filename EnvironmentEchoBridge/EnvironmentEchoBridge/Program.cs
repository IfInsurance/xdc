using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Installation.Environments;
using System;
using System.Configuration;

namespace EnvironmentEchoBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = null;

            Configure.Serialization.Json();
            var config = Configure.With()
                .DefineEndpointName("OnPremiseService1.EchoServiceBridge")
                .DefaultBuilder()
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .MsmqSubscriptionStorage()
                .UseInMemoryGatewayPersister()
                .UseInMemoryTimeoutPersister()
                .InMemorySagaPersister()
                .UseTransport<Msmq>()
                .Log4Net();

            using (var lifetimeManagement = config.UnicastBus().CreateBus())
            {
                bus = lifetimeManagement.Start(() => config.ForInstallationOn<Windows>().Install());

                ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;
                var appSettingsReader = new AppSettingsReader();
                var connectionString = (string)appSettingsReader.GetValue("Microsoft.ServiceBus.ConnectionString", typeof(string));

                var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
                var messageSender = messagingFactory.CreateMessageSender("CloudService1.EchoMessageHandler");
                Configure.Component<MessageSender>(() => messagingFactory.CreateMessageSender("CloudService1.EchoMessageHandler"), DependencyLifecycle.SingleInstance);

                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                if (!namespaceManager.SubscriptionExists("CloudService1.EchoMessageHandler.Events", "OnPremiseService1.EnvironmentMessageHandler"))
                {
                    namespaceManager.CreateSubscription("CloudService1.EchoMessageHandler.Events", "OnPremiseService1.EnvironmentMessageHandler");
                }
                var subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString, "CloudService1.EchoMessageHandler.Events", "OnPremiseService1.EnvironmentMessageHandler");
                subscriptionClient.OnMessage(message => new RelayEchoResponse(bus, subscriptionClient).Handle(message));


                Console.Title = "Environment Echo Bridge - Demo - Press ENTER to exit";
                Console.ReadLine();

                lifetimeManagement.Shutdown();
            }
        }
    }
}
