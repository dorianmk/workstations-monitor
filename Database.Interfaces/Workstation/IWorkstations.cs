using Database.Interfaces.Common;

namespace Database.Interfaces.Workstation
{
    public interface IWorkstations : IDbCollection<IWorkstation>
    {
        IWorkstation AddWorkstation(string name);
    }
}
