using Database.Interfaces.Map.Item;

namespace Database.MongoDB.Map.Item
{
    internal class ImageItem : MapItem, IImageItem
    {
        public byte[] File { get; private set; }

        internal ImageItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, byte[] file)
            : base(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer)
        {
            File = file;
        }
    }
}
