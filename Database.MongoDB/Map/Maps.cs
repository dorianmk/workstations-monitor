using Database.Interfaces.Map;
using Database.Interfaces.Map.Item;
using Database.MongoDB.Common;
using Database.MongoDB.Map.Item;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Database.MongoDB.Map
{
    internal class Maps : DbCollectionBase<IMap, Map>, IMaps
    {
        internal Maps(IMongoDatabase db)
               : base(db, "maps")
        {
            BsonClassMap.RegisterClassMap<ImageItem>();
            BsonClassMap.RegisterClassMap<WorkstationItem>();
        }

        public IMap CreateMap(string name, string id)
        {
            var result = new Map(name, id);
            return result;
        }

        public IImageItem CreateImageItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, byte[] file)
        {
            return new ImageItem(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer, file);
        }

        public IWorkstationItem CreateWorkstationItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, string workstationId)
        {
            return new WorkstationItem(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer, workstationId);
        }
    }
}
