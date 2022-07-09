using DataTransfer.Interfaces;

namespace DataTransfer.Tcp.Serializers
{
    public interface ISerializer
    {
        byte[] GetBytes(IData data);
        IData GetData(byte[] bytes);
    }
}
