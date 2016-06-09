using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Installation.Environments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentEchoBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Serialization.Json();
            var config = Configure.With()
                .DefineEndpointName("OnPremiseService1.EchoServiceBridge")
                .DefaultBuilder()
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .MsmqSubscriptionStorage()
                .UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());

                Microsoft.ServiceBus.ServiceBusEnvironment.SystemConnectivity.Mode = Microsoft.ServiceBus.ConnectivityMode.Https;
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

                Console.Title = "Environment Echo Bridge";
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
