using System;
using NServiceBus;
using OnPremiseService1.Public;
using NServiceBus.Installation.Environments;
using OnPremiseService2.Public;

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
                .DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.Contains("Public") && t.Name != null && t.Name.EndsWith("ed"))
                .DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.Contains("Public") && t.Name != null && (t.Name.StartsWith("Set") || t.Name.StartsWith("Mutate")))
                .MsmqSubscriptionStorage()
                .UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());
                bus.Send<MutateValue>(v => {
                    v.Operator = Operator.Add;
                    v.Operand = 5;
                });
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }

    public class EnvironmentHandler : IHandleMessages<SetEnvironmentVariable>
    {
        public IBus Bus { get; set; }

        public void Handle(SetEnvironmentVariable message)
        {
            var name = message.Name;
            if (string.IsNullOrEmpty(name))
                return;

            var currentValue = Environment.GetEnvironmentVariable(name);
            // business logic ... 
            Bus.Publish<EnvironmentVariableChanged>(evc => {
                evc.Name = name;
                evc.OldValue = currentValue;
                evc.Value = message.Value;
            });
        }
    }
}
