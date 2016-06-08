using Commands = CloudService1.Public.Commands;
using Events = CloudService1.Public.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NServiceBus;

namespace CloudService1.EchoMessageHandler
{
    public class CommandHandler : IHandleMessages<Commands.PleaseRepeatThis>
    {
        public async Task Handle(Commands.PleaseRepeatThis command, IMessageHandlerContext context)
        {
            if (String.IsNullOrEmpty(command.Phrase))
                return;

            await context.Publish<Events.EchoedResponse>(response => {
                response.EchoedPhrase = command.Phrase;
            });
        }
    }
}
