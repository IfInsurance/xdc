using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.AzureServiceBus.Interoperability;
using OnPremiseService2.Public.Events;
using System.Transactions;

namespace MathColorBridge
{
    public class RelayMathResponse : IHandleMessages<OnPremiseService2.Public.Events.ResultChanged>
    {
        public IBus Bus { get; set; }
        public MessageSender MessageSender { get; set; }

        public void Handle(ResultChanged message)
        {
            DemoPrintouts.Begin("Relaying Result Changed Event ... ");

            var brokeredMessage = Interop.CreateMessage(message, Bus.CurrentMessageContext.Id);

            // Break free from the local transaction and unconditionally send this message.
            // If it fails (throws an exception, we'll try again)
            // If it succeeds AND throws an exception, ASB's Duplicate message detection will take discard the duplicate message
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                MessageSender.Send(brokeredMessage);
                scope.Complete();
                DemoPrintouts.End("Done!");
            }
        }
    }
}
