using System.Collections.Generic;

namespace Diagnostics.Interfaces
{
    public interface INetwork
    {
        List<string> IpAdresses { get; }
    }
}
