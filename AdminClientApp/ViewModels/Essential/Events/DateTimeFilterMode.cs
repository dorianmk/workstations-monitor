using System.ComponentModel;

namespace AdminClientApp.ViewModels.Essential.Events
{
    public enum DateTimeFilterMode
    {
        [Description("Last hour")]
        LastHour,
        [Description("Last 12 hours")]
        Last12Hours,
        [Description("Last 24 hours")]
        Last24Hours,
        [Description("Last 7 days")]
        Last7Days,
        [Description("Last 30 days")]
        Last30Days,
        [Description("Any date")]
        AnyDate,
    }
}
