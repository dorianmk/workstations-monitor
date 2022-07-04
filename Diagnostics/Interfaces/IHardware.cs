
namespace Diagnostics.Interfaces
{
    public interface IHardware
    {
        string Processor { get; }
        string Motherboard { get; }
        string RAM { get; }
        IGPUInfo GPU1 { get; }
        IGPUInfo GPU2 { get; }
    }
}
