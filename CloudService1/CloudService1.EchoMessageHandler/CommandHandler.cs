using Commands = CloudService1.Public.Commands;
using Events = CloudService1.Public.Events;
using System;
using System.Threading.Tasks;
using NServiceBus;
using CloudService2.Public.Commands;

namespace CloudService1.EchoMessageHandler
{
    public class CommandHandler : IHandleMessages<Commands.PleaseRepeatThis>
    {
        public async Task Handle(Commands.PleaseRepeatThis command, IMessageHandlerContext context)
        {
            if (String.IsNullOrEmpty(command.Phrase))
                return;

            Console.WriteLine("Handling command PleaseRepeatThis");

            await context.Publish<Events.EchoedResponse>(response => {
                response.EchoedPhrase = command.Phrase;
            });

            await context.Send<TranslateColorNameToRgb>("CloudService2.ColorMessageHandler", translate => {
                translate.ColorName = command.Phrase;
                translate.CommandId = Guid.NewGuid();
            });
        }
    }
}
