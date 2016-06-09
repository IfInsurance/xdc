using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;
using Subscriptions = CloudService2.ColorMessageHandler.Models.SubscriptionContracts;

namespace CloudService2.ColorMessageHandler
{
    public static class EchoSubscriptionHandler
    {
        public static async Task Handle(
            [ServiceBusTrigger("CloudService1.EchoMessageHandler.Events", "CloudService2.ColorMessageHandler")]
            BrokeredMessage message)
        {
            var echoedResponse = await message.To<Subscriptions.EchoResponse>();

            Console.WriteLine($@"Got echo response: {echoedResponse.EchoedPhrase}");
        }
    }
}
