using NServiceBus;
using System;
using System.Threading.Tasks;
using CloudService2.Public.Events;
using Newtonsoft.Json;

namespace CloudService1.EchoMessageHandler
{
    public class ColorSubscription : IHandleMessages<ColorNameToRgbTranslationComplete>
    {
        public Task Handle(ColorNameToRgbTranslationComplete message, IMessageHandlerContext context)
        {
            Console.WriteLine("Got color name to RGB translation response: " + JsonConvert.SerializeObject(message));
            return Task.FromResult(true);
        }
    }
}
