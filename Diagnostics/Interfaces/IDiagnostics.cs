
namespace Diagnostics.Interfaces
{
    public interface IDiagnostics
    {
        IProcessesInfoProvider ProcessesInfoProvider { get; }
        ISystemInfoProvider SystemInfoProvider { get; }
    }
}
