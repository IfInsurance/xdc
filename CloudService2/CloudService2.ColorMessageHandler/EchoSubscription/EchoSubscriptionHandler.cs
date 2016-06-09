using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;
using WebJobs.NServiceBus.AzureServiceBus;

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
