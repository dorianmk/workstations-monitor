using Database.Interfaces.Map;
using Database.Interfaces.Map.Item;
using Database.MongoDB.Common;
using Database.MongoDB.Map.Item;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Database.MongoDB.Map
{
    internal class Map : IMap, IMongoEntity
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public List<MapItem> MapItems { get; private set; }

        internal Map(string name, string id)
        {
            Name = name;
            Id = string.IsNullOrEmpty(id) ? ObjectId.GenerateNewId() : new ObjectId(id);
            MapItems = new List<MapItem>();
        }

        public void AddItem(IMapItem item)
        {
            MapItems.Add(item as MapItem);
        }

        public string GetId() => Id.ToString();
        public IEnumerable<IMapItem> Items => MapItems.AsEnumerable();
    }
}
