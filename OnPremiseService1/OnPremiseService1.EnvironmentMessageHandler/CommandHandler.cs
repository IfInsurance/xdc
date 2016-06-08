using NServiceBus;
using System;
using Commands = OnPremiseService1.Public.Commands;
using Events = OnPremiseService1.Public.Events;

namespace OnPremiseService1.EnvironmentMessageHandler
{
    public class CommandHandler : IHandleMessages<Commands.SetEnvironmentVariable>
    {
        public IBus Bus { get; set; }

        public void Handle(Commands.SetEnvironmentVariable message)
        {
            var name = message.Name;
            if (string.IsNullOrEmpty(name))
                return;

            var currentValue = Environment.GetEnvironmentVariable(name);
            // business logic ... 
            Bus.Publish<Events.EnvironmentVariableChanged>(evc => {
                evc.Name = name;
                evc.OldValue = currentValue;
                evc.Value = message.Value;
            });
        }
    }
}
