
namespace Diagnostics.Interfaces
{
    public interface IGPUInfo
    {
        string Name { get; }
        uint MemoryMB { get; }
        bool IsActive { get; }
    }
}
