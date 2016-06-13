using NServiceBus;
using NServiceBus.Installation.Environments;
using Commands = OnPremiseService1.Public.Commands;
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
            config.Log4Net();
            config.UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());
                bus.Send<Commands.SetEnvironmentVariable>(variable =>
                {
                    variable.Name = "USER";
                    variable.Value = "Benjamin";
                });
                Console.WriteLine("Press any key to exit");
                Console.Out.Flush();
                Console.ReadKey();
            }
        }
    }
}
