using Database.Interfaces.Common;

namespace Database.Interfaces.Workstation
{
    public interface IWorkstation : IEntity
    {
        string Name { get; }
    }
}
