using CloudService1.Public.Events;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.AzureServiceBus.Interoperability;

namespace EnvironmentEchoBridge
{
    public class RelayEchoResponse
    {
        private IBus _bus;
        private SubscriptionClient _subscriptionClient;

        public RelayEchoResponse(IBus bus, SubscriptionClient subscriptionClient)
        {
            _bus = bus;
            _subscriptionClient = subscriptionClient;
        }

        public void Handle(BrokeredMessage message)
        {
            var echoedResponse = message.To<EchoedResponse>().Result;
            _bus.Send(message);
            message.Complete();
        }
    }
}
