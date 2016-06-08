using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using OnPremiseService2.Public;
using Commands = OnPremiseService2.Public.Commands;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Serialization.Json();
            var config = Configure.With()
                .DefineEndpointName("OnPremiseService1.EnvironmentMessageHandler")
                .DefaultBuilder()
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .MsmqSubscriptionStorage()
                .UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());
                bus.Send<Commands.MutateValue>(v => {
                    v.Operator = Operator.Add;
                    v.Operand = 5;
                });
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }

    
}
