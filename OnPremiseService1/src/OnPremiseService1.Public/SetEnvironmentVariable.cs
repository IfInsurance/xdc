using System;

namespace OnPremiseService1.Public.Commands
{
    public interface SetEnvironmentVariable
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}

namespace OnPremiseService1.Public.Events
{
    public interface EnvironmentVariableChanged
    {
        string Name { get; set; }
        string OldValue { get; set; }
        string Value { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}
