using System.Collections.Generic;

namespace Diagnostics.Interfaces
{
    public interface ISystemInfoProvider
    {
        string ComputerName { get; }
        IEnumerable<string> Applications { get; }
        IHardware Hardware { get; }
        INetwork Network { get; }
    }
}
