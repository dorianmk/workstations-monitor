using System;
using System.Collections.Generic;

namespace Diagnostics.Interfaces
{
    public interface IProcessesInfoProvider
    {
        event EventHandler<IEnumerable<IProcessInfo>> Actual;
    }
}