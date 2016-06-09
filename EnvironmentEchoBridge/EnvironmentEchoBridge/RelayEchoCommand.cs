using NServiceBus;
using CloudService1.Public.Commands;
using NServiceBus.AzureServiceBus.Interoperability;
using Microsoft.ServiceBus.Messaging;

namespace EnvironmentEchoBridge
{
    public class RelayEchoCommand : IHandleMessages<PleaseRepeatThis>
    {
        public IBus Bus { get; set; }
        public MessageSender MessageSender { get; set; }

        public void Handle(PleaseRepeatThis message)
        {
            var brokeredMessage = Interop.CreateMessage(message, Bus.CurrentMessageContext.Id);
            MessageSender.Send(brokeredMessage);
        }
    }
}
