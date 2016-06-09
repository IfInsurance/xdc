using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService2.ColorMessageHandler.Models.SubscriptionContracts
{
    public class EchoResponse : CloudService1.Public.Events.EchoedResponse
    {
        public string EchoedPhrase { get; set; }
    }
}
