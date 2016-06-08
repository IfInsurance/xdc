using Microsoft.Azure.WebJobs;
//using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subscriptions = CloudService2.Public.Events;

namespace CloudService1.EchoMessageHandler
{
    //public class ColorSubscriptionHandler : IHandleMessages<Subscriptions.ColorNameToRgbTranslationComplete>
    public static class ColorSubscriptionHandler
    {
        //public Task Handle(Subscriptions.ColorNameToRgbTranslationComplete message, IMessageHandlerContext context)
        public static async Task Handle(
            [ServiceBusTrigger("CloudService2.ColorMessageHandler.Events")]
            Subscriptions.ColorNameToRgbTranslationComplete message)
        {
            Console.WriteLine($@"Got subscription response:
    Event Id: {message.EventId.ToString("N")}
    In Response To: {message.InResponseToCommandId.ToString("N")}
    Translation: rgb({message.Red}, {message.Green}, {message.Blue})
    Translation was done: {message.Timestamp.ToLocalTime().ToString()}");
        }
    }
}
