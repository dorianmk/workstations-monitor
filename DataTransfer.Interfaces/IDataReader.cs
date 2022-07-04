using System;

namespace DataTransfer.Interfaces
{
    public interface IDataReader
    {
        event EventHandler<IData> OnRead;
    }
}
