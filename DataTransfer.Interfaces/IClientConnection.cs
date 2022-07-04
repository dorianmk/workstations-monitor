﻿using System;
using System.Threading.Tasks;

namespace DataTransfer.Interfaces
{
    public interface IClientConnection
    {
        IDataTwoWay Server { get; }
        event EventHandler Connected;
        event EventHandler<Exception> Stopped;
        bool IsConnected { get; }
        Task Start();
    }
}
