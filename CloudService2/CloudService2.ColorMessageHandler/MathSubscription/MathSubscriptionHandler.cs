using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using NServiceBus.AzureServiceBus.Interoperability;
using OnPremiseService2.Public.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService2.ColorMessageHandler.MathSubscription
{
    public static class MathSubscriptionHandler
    {
        public static async Task Handle(
            [ServiceBusTrigger("OnPremiseService2.MathColorBridge.Events", "CloudService2.ColorMessageHandler")]
            BrokeredMessage message,
            
            [ServiceBus("CloudService2.ColorMessageHandler.Events")]
            IAsyncCollector<BrokeredMessage> colorTranslationEvents)
        {
            var mathResult = await message.To<MathResult>();

            var color = System.Drawing.Color.FromArgb((int)mathResult.Result);
            var colorTranslation = new ColorTranslation.OutputModel {
                Red = color.R,
                Green = color.G,
                Blue = color.B,
            };
            var colorEvent = Interop.CreateMessage<Public.Events.ColorNameToRgbTranslationComplete>(colorTranslation);
            await colorTranslationEvents.AddAsync(colorEvent);
        }

        class MathResult : ResultChanged
        {
            public decimal Result { get; set; }
            public DateTimeOffset Timestamp { get; set; }
        }
    }
}
