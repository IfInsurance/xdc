using NServiceBus;
using NServiceBus.Installation.Environments;
using System;
using OnPremiseService2.Public;
using OnPremiseService1.Public;

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
            config.DefiningEventsAs(t => t != null && t.Namespace != null && t.Namespace.Contains("Public") && t.Name != null && t.Name.EndsWith("ed"));
            config.DefiningCommandsAs(t => t != null && t.Namespace != null && t.Namespace.Contains("Public") && t.Name != null && (t.Name.StartsWith("Set") || t.Name.StartsWith("Mutate")));
            config.MsmqSubscriptionStorage();
            config.UseTransport<Msmq>();

            using (var bus = config.UnicastBus().CreateBus())
            {
                bus.Start(() => config.ForInstallationOn<Windows>().Install());
                bus.Send<SetEnvironmentVariable>(value => { value.Name = "USER"; value.Value = "Benjamin"; });
                Console.WriteLine("Press any key to exit");
                Console.Out.Flush();
                Console.ReadKey();
            }
        }
    }

    public class MathMessageHandler : IHandleMessages<MutateValue>
    {
        public decimal CurrentValue { get; set; }
        public IBus Bus { get; set; }

        public void Handle(MutateValue value)
        {
            switch (value.Operator)
            {
                case Operator.Add:
                    CurrentValue += value.Operand;
                    break;
                case Operator.Remove:
                    CurrentValue -= value.Operand;
                    break;
                case Operator.Multiply:
                    CurrentValue *= value.Operand;
                    break;
                case Operator.Divide:
                    if (CurrentValue == 0 || value.Operator == 0)
                        return;
                    CurrentValue /= value.Operand;
                    break;
                case Operator.Reset:
                    if (value.Operator != 0)
                        return;
                    CurrentValue = 0;
                    break;
                default:
                    return;
            }

            Bus.Publish<ResultChanged>(change =>
            {
                change.Result = CurrentValue;
                change.Timestamp = DateTimeOffset.Now;
            });
        }
    }
}
