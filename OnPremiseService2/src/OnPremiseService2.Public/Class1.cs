using System;

namespace OnPremiseService2.Public
{
    public enum Operator
    {
        Add, Remove, Multiply, Divide, Reset
    }

    public interface MutateValue
    {
        Operator Operator { get; set; }
        decimal Operand { get; set; }
    }

    public interface ResultChanged
    {
        decimal Result { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}
