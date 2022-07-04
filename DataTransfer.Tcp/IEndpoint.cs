
namespace DataTransfer.Tcp
{
    public interface IEndpoint
    {
        string Hostname { get; }
        int Port { get; }
    }
}
