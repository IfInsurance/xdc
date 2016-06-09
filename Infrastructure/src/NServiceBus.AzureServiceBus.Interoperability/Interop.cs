using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.AzureServiceBus.Interoperability
{
    public static class Interop
    {
        public static BrokeredMessage CreateMessage<OfType>(OfType instance, Guid? messageId = null, Guid? responseId = null)
        {
            var jsonSerializedInstance = JsonConvert.SerializeObject(instance);
            var byteRepresentation = Encoding.UTF8.GetBytes(jsonSerializedInstance);
            var bodyStream = new MemoryStream(byteRepresentation);
            bodyStream.Seek(0, SeekOrigin.Begin);

            var resultMessage = new BrokeredMessage(bodyStream, true);
            resultMessage.Properties.Add("NServiceBus.Transport.Encoding", "application/octect-stream");
            resultMessage.Properties.Add("NServiceBus.ContentType", "text/json");
            resultMessage.Properties.Add("NServiceBus.EnclosedMessageTypes", 
                string.Join(",", instance.GetType().GetInterfaces().Select(i => i.FullName)));
            resultMessage.Properties.Add("NServiceBus.MessageId", (messageId.HasValue ? messageId : Guid.NewGuid()).ToString());
            if (responseId.HasValue)
            {
                resultMessage.Properties.Add("NServiceBus.RelatedTo", responseId.Value.ToString());
            }
            
            resultMessage.Properties.Add("NServiceBus.OriginatingEndpoint", Assembly.GetExecutingAssembly().FullName);
            resultMessage.Properties.Add("NServiceBus.TimeSent", DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:ffffff Z"));
            return resultMessage;
        }

        public static BrokeredMessage AsEvent(this BrokeredMessage message)
        {
            message.Properties.Add("NServiceBus.MessageIntent", "Publish");
            return message;
        }

        public static BrokeredMessage AsCommand(this BrokeredMessage message)
        {
            message.Properties.Add("NServiceBus.MessageIntent", "Send");
            return message;
        }

        public static async Task<TargetModel> To<TargetModel>(this BrokeredMessage message)
        {
            var transportEncoding = (string)message.Properties["NServiceBus.Transport.Encoding"];
            string messageContent;

            if (transportEncoding == "wcf/byte-array")
            {
                messageContent = Encoding.Default.GetString(message.GetBody<byte[]>());
            }
            else if (transportEncoding == "application/octect-stream")
            {
                using (var bodyStream = message.GetBody<Stream>())
                using (var reader = new StreamReader(bodyStream))
                    messageContent = await reader.ReadToEndAsync();
            }
            else throw new InvalidOperationException("Cannot interpret transport encoding " + transportEncoding);

            var model = JsonConvert.DeserializeObject<TargetModel>(messageContent);
            return model;
        }
    }
}
