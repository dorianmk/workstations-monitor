using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface IProvider<T>
    {
        IReadOnlyCollection<T> Items { get; }
        event EventHandler<T> ItemAdded;
    }
}
