using NServiceBus;
using Subscriptions = OnPremiseService2.Public.Events;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class MathSubscription : IHandleMessages<Subscriptions.ResultChanged>
    {
        public void Handle(Subscriptions.ResultChanged message)
        {
            DemoPrintouts.End("Received an math response: " + message.Result);
        }
    }
}
