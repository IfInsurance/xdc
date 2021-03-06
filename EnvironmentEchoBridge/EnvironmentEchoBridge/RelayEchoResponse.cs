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
            DemoPrintouts.Begin("Relaying Echo Response Event ... ");
            var echoedResponse = message.To<EchoedResponseModel>().Result;
            foreach (var property in message.Properties)
            {
                _bus.SetMessageHeader(echoedResponse, property.Key, property.Value.ToString());
            }

            _bus.Publish<EchoedResponse>(echoedResponse);
            message.Complete();
            DemoPrintouts.End("Done!");
        }

        class EchoedResponseModel : EchoedResponse, IEvent
        {
            public string EchoedPhrase { get; set; }
        }
    }
}
