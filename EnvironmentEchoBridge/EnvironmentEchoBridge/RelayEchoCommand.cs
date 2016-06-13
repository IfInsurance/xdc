using NServiceBus;
using CloudService1.Public.Commands;
using NServiceBus.AzureServiceBus.Interoperability;
using Microsoft.ServiceBus.Messaging;
using System.Transactions;

namespace EnvironmentEchoBridge
{
    public class RelayEchoCommand : IHandleMessages<PleaseRepeatThis>
    {
        public IBus Bus { get; set; }
        //public MessageSender MessageSender { get; set; }

        public void Handle(PleaseRepeatThis message)
        {
            System.Console.Write("Relaying Echo Command ... ");

            var appSettingsReader = new System.Configuration.AppSettingsReader();
            var connectionString = (string)appSettingsReader.GetValue("Microsoft.ServiceBus.ConnectionString", typeof(string));

            var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
            var MessageSender = messagingFactory.CreateMessageSender("CloudService1.EchoMessageHandler");
            //Configure.Component<MessageSender>(() => messagingFactory.CreateMessageSender("CloudService1.EchoMessageHandler"), DependencyLifecycle.SingleInstance);

            var brokeredMessage = Interop.CreateMessage(message, Bus.CurrentMessageContext.Id);

            // Break free from the local transaction and unconditionally send this message.
            // If it fails (throws an exception, we'll try again)
            // If it succeeds AND throws an exception, ASB's Duplicate message detection will take discard the duplicate message
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                MessageSender.Send(brokeredMessage);
                scope.Complete();
                MessageSender.Close();
                System.Console.WriteLine("Done!");
            }
        }
    }
}
