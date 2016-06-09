using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudService2.Public.Events;
using Newtonsoft.Json;

namespace CloudService1.EchoMessageHandler
{
    public class ColorSubscription : IHandleMessages<CloudService2.Public.Events.ColorNameToRgbTranslationComplete>
    {
        public Task Handle(ColorNameToRgbTranslationComplete message, IMessageHandlerContext context)
        {
            Console.WriteLine("Got response: " + JsonConvert.SerializeObject(message));
            return Task.FromResult(true);
        }
    }
}
