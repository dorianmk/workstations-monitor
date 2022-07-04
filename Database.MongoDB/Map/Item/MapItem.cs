using Database.Interfaces.Map.Item;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.Map.Item
{
    internal class MapItem : IMapItem, IMongoEntity
    {
        public ObjectId Id { get; private set; }
        public double RelativeTop { get; private set; }
        public double RelativeLeft { get; private set; }
        public double RelativeWidth { get; private set; }
        public double RelativeHeight { get; private set; }
        public int Layer { get; private set; }

        protected MapItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer)
        {
            RelativeTop = relativeTop;
            RelativeLeft = relativeLeft;
            RelativeWidth = relativeWidth;
            RelativeHeight = relativeHeight;
            Layer = layer;
        }

        public string GetId() => Id.ToString();
    }
}
