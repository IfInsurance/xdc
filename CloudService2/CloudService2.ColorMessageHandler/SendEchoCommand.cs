using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using WebJobs.NServiceBus.AzureServiceBus;

namespace CloudService2.ColorMessageHandler
{
    public static class SendEchoCommand
    {
        public static async Task Example(
            [ServiceBus("CloudService1.EchoMessageHandler")]
            IAsyncCollector<BrokeredMessage> repeats)
        {
            var model = new PleaseRepeatThisCommandModel { Phrase = "I'm a lumberjack and I'm OK" };
            var message = Interop.CreateMessage(model).AsCommand();
            await repeats.AddAsync(message);
        }

        class PleaseRepeatThisCommandModel : CloudService1.Public.Commands.PleaseRepeatThis
        {
            public string Phrase { get; set; }
        }
    }
}
