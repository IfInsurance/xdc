using NServiceBus;
using System;
using CloudService1.Public.Events;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class EchoSubscription : IHandleMessages<EchoedResponse>
    {
        public void Handle(EchoedResponse message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Received an echo response: " + message.EchoedPhrase);
            Console.ResetColor();
        }
    }
}
