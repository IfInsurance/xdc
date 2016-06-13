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
            IBus messaging = null;
            Configure.Serialization.Json();
            var config = Configure.With()
                .DefineEndpointName("OnPremiseService1.EnvironmentMessageHandler")
                .DefaultBuilder()
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Events"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.EndsWith("Public.Commands"))
                .MsmqSubscriptionStorage()
                .InMemorySagaPersister()
                .UseInMemoryGatewayPersister()
                .UseInMemoryTimeoutPersister()
                .UseTransport<Msmq>()
                .Log4Net();

            using (var lifetimeManagement = config.UnicastBus().CreateBus())
            {
                messaging = lifetimeManagement.Start(() => config.ForInstallationOn<Windows>().Install());

                Console.Title = "OnPremiseService1 - Demo - Inputs: '1', '2', 'q'";
                ConsoleKeyInfo consoleKey;

                do
                {
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.KeyChar)
                    {
                        case '1':
                            EnvironmentEchoBridge.DemoPrintouts.Begin("Sending command ... ");
                            messaging.Send<Commands.MutateValue>(v =>
                            {
                                v.Operator = Operator.Add;
                                v.Operand = 5;
                            });
                            break;
                        case '2':
                            EnvironmentEchoBridge.DemoPrintouts.Begin("Sending command ... ");
                            messaging.Send<CloudService1.Public.Commands.PleaseRepeatThis>(r =>
                            {
                                r.Phrase = "Magenta";
                            });
                            break;
                        default:
                            break;
                    }

                } while (consoleKey.Key != ConsoleKey.Q);


                lifetimeManagement.Shutdown();
            }
        }
    }
}
