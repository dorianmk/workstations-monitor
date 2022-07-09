using System;

namespace DataTransfer.Interfaces
{
    public interface IConnection : IDataTwoWay
    {
        event EventHandler Stopped;
    }
}
