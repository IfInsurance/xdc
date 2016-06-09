using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands = CloudService2.Public.Commands;
using Events = CloudService2.Public.Events;

namespace CloudService2.ColorMessageHandler.Models.ColorTranslation
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
}
