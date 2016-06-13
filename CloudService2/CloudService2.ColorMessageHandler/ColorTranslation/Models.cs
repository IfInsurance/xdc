using System;
using OnPremiseService2.Public;
using Commands = CloudService2.Public.Commands;
using Events = CloudService2.Public.Events;

namespace CloudService2.ColorMessageHandler.ColorTranslation
{
    public class InputModel : Commands.TranslateColorNameToRgb
    {
        public string ColorName { get; set; }
        public Guid CommandId { get; set; }
    }

    public class OutputModel : Events.ColorNameToRgbTranslationComplete
    {
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
        public Guid EventId { get; set; }
        public Guid InResponseToCommandId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class MathModel : OnPremiseService2.Public.Commands.MutateValue
    {
        public decimal Operand { get; set; }
        public Operator Operator { get; set; }
    }
}
