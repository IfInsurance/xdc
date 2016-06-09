//using NServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using ColorTranslation = CloudService2.ColorMessageHandler.Models.ColorTranslation;

namespace CloudService2.ColorMessageHandler
{
    //public class CommandHandler : IHandleMessages<Commands.TranslateColorNameToRgb>
    public static class CommandHandler
    {
        //public IBus Bus { get; set; }

        //public void Handle(Commands.TranslateColorNameToRgb message)
        public static async Task Handle(
            [ServiceBusTrigger("CloudService2.ColorMessageHandler")]
            BrokeredMessage message,
            
            [ServiceBus("CloudService2.ColorMessageHandler.Events")]
            IAsyncCollector<BrokeredMessage> events)
        {
            Console.WriteLine("Handling command TranslateColorNameToRgb");

            var bodyStream = message.GetBody<System.IO.Stream>();
            var reader = new System.IO.StreamReader(bodyStream);
            var bodyText = await reader.ReadToEndAsync();
            reader.Dispose();

            var inputModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ColorTranslation.InputModel>(bodyText);

            if (String.IsNullOrEmpty(inputModel.ColorName))
                return;

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

            var resultMessage = NServiceBusMessageFactory
                .CreateMessage(responseModel, responseModel.EventId, responseModel.InResponseToCommandId)
                .AsEvent();

            await events.AddAsync(resultMessage);
        }
    }
}
