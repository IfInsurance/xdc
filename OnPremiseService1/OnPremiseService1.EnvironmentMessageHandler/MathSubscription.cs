using NServiceBus;
using System;
using Subscriptions = OnPremiseService2.Public.Events;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class MathSubscription : IHandleMessages<Subscriptions.ResultChanged>
    {
        public void Handle(Subscriptions.ResultChanged message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Received a Result Changed response: " + message.Result);
            Console.ResetColor();
        }
    }
}
