using Database.Interfaces.Common;
using Database.Interfaces.Map.Item;

namespace Database.Interfaces.Map
{
    public interface IMaps : IDbCollection<IMap>
    {
        IMap CreateMap(string name, string id = null);
        IImageItem CreateImageItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, byte[] file);
        IWorkstationItem CreateWorkstationItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, string workstationId);
    }
}
