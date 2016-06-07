using NServiceBus;
using OnPremiseService1.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnPremiseService2.MathMessageHandler
{
    public class EnvironmentStatusSubscription : IHandleMessages<EnvironmentVariableChanged>
    {
        public void Handle(EnvironmentVariableChanged message)
        {
            Console.WriteLine($"Got info that environment variable {message.Name} was changed from {message.OldValue} to {message.Value}!");
        }
    }
}
