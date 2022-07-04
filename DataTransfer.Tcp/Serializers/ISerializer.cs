using System.Collections.Generic;
using DataTransfer.Interfaces;

namespace DataTransfer.Tcp.Serializers
{
    public interface ISerializer
    {
        byte[] GetBytes(IData data);
        List<IData> GetDatas(byte[] bytes);
    }
}
