using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Installation.Environments;
using System;
using System.Configuration;

namespace MathColorBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = null;

            Configure.Serialization.Json();
            var config = Configure.With()
                .DefineEndpointName("OnPremiseService2.ColorServiceBridge")
                .DefaultBuilder()
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .MsmqSubscriptionStorage()
                .UseInMemoryGatewayPersister()
                .UseInMemoryTimeoutPersister()
                .InMemorySagaPersister()
                .UseTransport<Msmq>()
                /*.Log4Net()*/;

            using (var lifetimeManagement = config.UnicastBus().CreateBus())
            {
                bus = lifetimeManagement.Start(() => config.ForInstallationOn<Windows>().Install());

                ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;
                var appSettingsReader = new AppSettingsReader();
                var connectionString = (string)appSettingsReader.GetValue("Microsoft.ServiceBus.ConnectionString", typeof(string));

                // Command: From Cloud to Premise
                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                if (!namespaceManager.QueueExists("OnPremiseService2.MathColorBridge"))
                {
                    namespaceManager.CreateQueue("OnPremiseService2.MathColorBridge");
                }
                var commandQueueClient = QueueClient.CreateFromConnectionString(connectionString, "OnPremiseService2.MathColorBridge");
                commandQueueClient.OnMessage(message => RelayMathCommand.Handle(message, bus));

                // Event: From Premise to Cloud
                if (!namespaceManager.TopicExists("OnPremiseService2.MathColorBridge.Events"))
                {
                    namespaceManager.CreateTopic("OnPremiseService2.MathColorBridge.Events");
                }
                var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
                Configure.Component<MessageSender>(
                    () => messagingFactory.CreateMessageSender("OnPremiseService2.MathColorBridge.Events"), 
                    DependencyLifecycle.SingleInstance);

                Console.Title = "Math Color Bridge - Demo - Press ENTER to exit";
                Console.ReadLine();

                lifetimeManagement.Shutdown();
            }
        }
    }
}
