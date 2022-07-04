using Database.Interfaces.Common;

namespace Database.Interfaces.Migration
{
    public interface IMigration : IEntity
    {
        int Number { get; }
        void Apply();
    }
}
