using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using NServiceBus.AzureServiceBus.Interoperability;
using System;
using System.Threading.Tasks;

namespace CloudService2.ColorMessageHandler.EchoSubscription
{
    public static class EchoSubscriptionHandler
    {
        public static async Task Handle(
            [ServiceBusTrigger("CloudService1.EchoMessageHandler.Events", "CloudService2.ColorMessageHandler")]
            BrokeredMessage message)
        {
            var echoedResponse = await message.To<EchoResponse>();

            Console.WriteLine($@"Got echo response: {echoedResponse.EchoedPhrase}");
        }
    }
}
