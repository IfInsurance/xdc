using NServiceBus;
using Events = OnPremiseService1.Public.Events;
using System;

namespace OnPremiseService2.MathMessageHandler
{
    public class EnvironmentStatusSubscription : IHandleMessages<Events.EnvironmentVariableChanged>
    {
        public void Handle(Events.EnvironmentVariableChanged message)
        {
            Console.WriteLine($"Got info that environment variable {message.Name} was changed from {message.OldValue} to {message.Value}!");
        }
    }
}
