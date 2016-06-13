using NServiceBus;
using NServiceBus.Installation.Environments;
using System;

namespace OnPremiseService2.MathMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Serialization.Json();
            var config = Configure.With();
            config.DefineEndpointName("OnPremiseService2.MathMessageHandler");
            config.DefaultBuilder();
            config.DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"));
            config.DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"));
            config.MsmqSubscriptionStorage();
            config.InMemorySagaPersister();
            config.UseInMemoryGatewayPersister();
            config.UseInMemoryTimeoutPersister();
            //config.Log4Net();
            config.UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());
                Console.Title = "OnPremiseService2 - Demo - Press ENTER to exit";
                Console.ReadLine();
            }
        }
    }
}
