using DataTransfer.Interfaces;
using System;

namespace Common.DataTransfer.RequestResponse
{
    public interface IDataWithId : IData
    {
        Guid Id { get; }
    }
}
