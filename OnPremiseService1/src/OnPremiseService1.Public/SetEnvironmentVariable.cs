using System;

namespace OnPremiseService1.Public
{
    public interface SetEnvironmentVariable
    {
        string Name { get; set; }
        string Value { get; set; }
    }

    public interface EnvironmentVariableChanged
    {
        string Name { get; set; }
        string OldValue { get; set; }
        string Value { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}
