using NServiceBus;
using System;
using OnPremiseService2.Public;
using Events = OnPremiseService2.Public.Events;
using Commands = OnPremiseService2.Public.Commands;

namespace OnPremiseService2.MathMessageHandler
{
    public class MathMessageHandler : IHandleMessages<Commands.MutateValue>
    {
        public decimal CurrentValue { get; set; }
        public IBus Bus { get; set; }

        public void Handle(Commands.MutateValue value)
        {
            switch (value.Operator)
            {
                case Operator.Add:
                    CurrentValue += value.Operand;
                    break;
                case Operator.Remove:
                    CurrentValue -= value.Operand;
                    break;
                case Operator.Multiply:
                    CurrentValue *= value.Operand;
                    break;
                case Operator.Divide:
                    if (CurrentValue == 0 || value.Operator == 0)
                        return;
                    CurrentValue /= value.Operand;
                    break;
                case Operator.Reset:
                    if (value.Operator != 0)
                        return;
                    CurrentValue = 0;
                    break;
                default:
                    return;
            }

            Bus.Publish<Events.ResultChanged>(change =>
            {
                change.Result = CurrentValue;
                change.Timestamp = DateTimeOffset.Now;
            });
        }
    }
}
