using Database.Interfaces.Common;
using Database.Interfaces.Map.Item;
using System.Collections.Generic;

namespace Database.Interfaces.Map
{
    public interface IMap : IEntity
    {
        string Name { get; }
        IEnumerable<IMapItem> Items { get; }
        void AddItem(IMapItem item);
    }
}
