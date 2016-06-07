using OnPremiseService2;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnPremiseService2.Public;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class MathSubscription : IHandleMessages<ResultChanged>
    {
        public void Handle(ResultChanged message)
        {
            Console.WriteLine("w00t! value: " + message.Result);
        }
    }
}
