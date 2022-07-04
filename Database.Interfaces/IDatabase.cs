using Database.Interfaces.Event;
using Database.Interfaces.EventRule;
using Database.Interfaces.Map;
using Database.Interfaces.Migration;
using Database.Interfaces.User;
using Database.Interfaces.Workstation;

namespace Database.Interfaces
{
    public interface IDatabase
    {
        bool CreateIfNeeded();
        IUsers Users { get; }
        IWorkstations Workstations { get; }
        IMigrations Migrations { get; }
        IMaps Maps {get; }
        IEvents Events { get; }
        IEventRules EventRules { get; }
    }
}
