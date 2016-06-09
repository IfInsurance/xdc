using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using EchoCommands = CloudService1.Public.Commands;

namespace CloudService2.ColorMessageHandler
{
    class Program
    {
        static void Main()
        {
            var hostConfiguration = new JobHostConfiguration();
            var serviceBusConfiguration = new ServiceBusConfiguration();
            serviceBusConfiguration.MessageOptions.MaxConcurrentCalls = 1;
            serviceBusConfiguration.MessageOptions.ExceptionReceived += (sender, args) => System.Diagnostics.Debugger.Break();
            hostConfiguration.UseServiceBus(serviceBusConfiguration);
            var host = new JobHost(hostConfiguration);
            host.Call(typeof(ExampleUseCase).GetMethod(nameof(ExampleUseCase.SendMessage)));
            host.RunAndBlock();
        }
    }

    public static class ExampleUseCase
    {
        public static async Task SendMessage(
            [ServiceBus("CloudService1.EchoMessageHandler")]
            IAsyncCollector<BrokeredMessage> repeats)
        {
            var message = NServiceBusMessageFactory
                .CreateMessage(new PrtDto { Phrase = "I'm a lumberjack and I'm OK" })
                .AsCommand();

            await repeats.AddAsync(message);
        }
    }

    public class PrtDto : EchoCommands.PleaseRepeatThis
    {
        public string Phrase { get; set; }
    }
}
