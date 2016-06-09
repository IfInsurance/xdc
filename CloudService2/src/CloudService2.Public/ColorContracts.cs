using System;

namespace CloudService2.Public.Commands
{
    public class TranslateColorNameToRgb
    {
        public Guid CommandId { get; set; }
        public string ColorName { get; set; }
    }
}

namespace CloudService2.Public.Events
{
    public class ColorNameToRgbTranslationComplete
    {
        public Guid EventId { get; set; }
        public Guid InResponseToCommandId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
