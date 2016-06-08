using System;

namespace OnPremiseService2.Public
{
    public enum Operator
    {
        Add, Remove, Multiply, Divide, Reset
    }
}

namespace OnPremiseService2.Public.Commands
{
    public interface MutateValue
    {
        Operator Operator { get; set; }
        decimal Operand { get; set; }
    }
}

namespace OnPremiseService2.Public.Events
{ 
    public interface ResultChanged
    {
        decimal Result { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}
