using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;

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
            host.Call(typeof(SendEchoCommand).GetMethod(nameof(SendEchoCommand.Example)));
            host.RunAndBlock();
        }
    }

}
