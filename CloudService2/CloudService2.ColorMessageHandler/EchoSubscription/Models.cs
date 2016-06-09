namespace CloudService2.ColorMessageHandler.EchoSubscription
{
    public class EchoResponse : CloudService1.Public.Events.EchoedResponse
    {
        public string EchoedPhrase { get; set; }
    }
}
