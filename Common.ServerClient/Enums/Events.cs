using System.ComponentModel;

namespace Common.Enums
{
    public enum EventType
    {
        Information,
        Warning,
        Error
    }

    public enum EventSource
    {
        CPU,
        Memory,
        TCP,
        Disk
    }

    public enum ComparisonMode
    {
        [Description(">")]
        Greater,
        [Description(">=")]
        GreaterOrEqual,
        [Description("<")]
        Less,
        [Description("<=")]
        LessOrEqual,
    }
}
