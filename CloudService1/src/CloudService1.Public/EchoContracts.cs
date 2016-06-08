namespace CloudService1.Public.Commands
{
    public interface PleaseRepeatThis
    {
        string Phrase { get; set; }
    }
}

namespace CloudService1.Public.Events
{
    public interface EchoedResponse
    {
        string EchoedPhrase { get; set; }
    }
}


