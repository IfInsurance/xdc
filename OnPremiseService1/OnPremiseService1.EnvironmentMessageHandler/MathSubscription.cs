using NServiceBus;
using System;
using Subscriptions = OnPremiseService2.Public.Events;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class MathSubscription : IHandleMessages<Subscriptions.ResultChanged>
    {
        public void Handle(Subscriptions.ResultChanged message)
        {
            Console.WriteLine("w00t! value: " + message.Result);
        }
    }
}