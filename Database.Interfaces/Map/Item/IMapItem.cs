using Database.Interfaces.Common;

namespace Database.Interfaces.Map.Item
{
    public interface IMapItem : IEntity
    {
        double RelativeTop { get; }
        double RelativeLeft { get; }
        double RelativeWidth { get; }
        double RelativeHeight { get; }
        int Layer { get; }
    }

    public interface IImageItem : IMapItem
    {
        byte[] File { get; }
    }

    public interface IWorkstationItem : IMapItem
    {
        string WorkstationId { get; }
    }
}
