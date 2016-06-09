using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using WebJobs.NServiceBus.AzureServiceBus;
using ColorTranslation = CloudService2.ColorMessageHandler.Models.ColorTranslation;

namespace CloudService2.ColorMessageHandler
{
    public static class CommandHandler
    {
        public static async Task Handle(
            [ServiceBusTrigger("CloudService2.ColorMessageHandler")]
            BrokeredMessage message,
            
            [ServiceBus("CloudService2.ColorMessageHandler.Events")]
            IAsyncCollector<BrokeredMessage> events)
        {
            Console.WriteLine("Handling command TranslateColorNameToRgb");

            var inputModel = await message.To<ColorTranslation.InputModel>();

            if (String.IsNullOrEmpty(inputModel.ColorName))
                return;

            ColorTranslation.OutputModel responseModel = Translate(inputModel);

            var resultMessage = Interop
                .CreateMessage(responseModel, responseModel.EventId, responseModel.InResponseToCommandId)
                .AsEvent();

            await events.AddAsync(resultMessage);
        }

        private static ColorTranslation.OutputModel Translate(ColorTranslation.InputModel inputModel)
        {
            int r = 0, g = 0, b = 0;
            try
            {
                var translatedColor = Color.FromName(inputModel.ColorName);
                r = translatedColor.R;
                g = translatedColor.G;
                b = translatedColor.B;
            }
            catch
            {
            }

            var responseModel = new ColorTranslation.OutputModel
            {
                InResponseToCommandId = inputModel.CommandId,
                Timestamp = DateTimeOffset.UtcNow,
                EventId = Guid.NewGuid(),
                Red = r,
                Green = g,
                Blue = b
            };
            return responseModel;
        }
    }
}
