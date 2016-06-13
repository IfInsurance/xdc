using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using OnPremiseService2.Public;
using Commands = OnPremiseService2.Public.Commands;
using System.Threading.Tasks;
using System.Threading;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus messaging = null;
            var nServiceBusInitialized = new ManualResetEvent(false);
            var waitHandle = new ManualResetEvent(false);

            var nServiceBusThread = new Thread(() => {
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

                    nServiceBusInitialized.Set();

                    waitHandle.WaitOne();

                    lifetimeManagement.Shutdown();
                }
            });

            var uiThread = new Thread(() => {
                Console.WriteLine($"Running {typeof(Program).FullName}");
                Console.WriteLine("Press 1 to send a '+5' command to the On-premise Math Service");
                Console.WriteLine("Press 2 to send a 'Magenta' command to the Echo Service in the Cloud");
                Console.WriteLine("Press q to exit");
                Console.WriteLine("Press any other key to allow NServiceBus to execute its handlers");

                ConsoleKeyInfo consoleKey;
                do
                {
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.KeyChar)
                    {
                        case '1':
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write('>');
                            Console.ResetColor();
                            messaging.Send<Commands.MutateValue>(v =>
                            {
                                v.Operator = Operator.Add;
                                v.Operand = 5;
                            });
                            break;
                        case '2':
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write('>');
                            Console.ResetColor();
                            messaging.Send<CloudService1.Public.Commands.PleaseRepeatThis>(r =>
                            {
                                r.Phrase = "Magenta";
                            });
                            break;
                        //case 'r':
                        //case 'R':
                        //    Console.ForegroundColor = ConsoleColor.Yellow;
                        //    Console.WriteLine("Reinitializing NServiceBus ... ");
                        //    cancellationTokenSource.Cancel();
                        //    while (messaging != null)
                        //        Thread.Yield();
                        //    nServiceBusThread.Start();
                        //    Console.ForegroundColor = ConsoleColor.Green;
                        //    Console.WriteLine("NServiceBus reinitialized!");
                        //    Console.ResetColor();
                        //    break;
                        case 'q':
                        case 'Q':
                            break;
                        case 'h':
                        case 'H':
                        case '?':
                            Console.WriteLine($"Running {typeof(Program).FullName}");
                            Console.WriteLine("Press 1 to send a '+5' command to the On-premise Math Service");
                            Console.WriteLine("Press 2 to send a 'Magenta' command to the Echo Service in the Cloud");
                            Console.WriteLine("Press q to exit");
                            break;
                        default:
                            break;
                    }

                } while (consoleKey.Key != ConsoleKey.Q);
            });

            Console.WriteLine("Starting NServiceBus...");
            nServiceBusThread.Start();
            nServiceBusInitialized.WaitOne();
            Console.WriteLine("NServiceBus initialized");

            uiThread.Start();
            uiThread.Join();
            waitHandle.Set();
        }
    }
}
