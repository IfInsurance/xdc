using NServiceBus;
using NServiceBus.AzureServiceBus.Interoperability;
using Microsoft.ServiceBus.Messaging;
using OnPremiseService2.Public.Commands;

namespace MathColorBridge
{
    public static class RelayMathCommand
    {
        public static void Handle(BrokeredMessage message, IBus bus)
        {
            DemoPrintouts.Begin("Relaying Math Command ... ");

            var mutateValueModel = message.To<MutateValueCommandModel>().Result;

            bus.Send<MutateValue>(value => {
                value.Operand = mutateValueModel.Operand;
                value.Operator = mutateValueModel.Operator;

                //foreach (var property in message.Properties)
                //{
                //    bus.SetMessageHeader(value, property.Key, property.Value.ToString());
                //}
            });

            message.Complete();
            DemoPrintouts.End("Done!");
        }

        class MutateValueCommandModel : MutateValue
        {
            public OnPremiseService2.Public.Operator Operator { get; set; }
            public decimal Operand { get; set; }
        }
    }
}
