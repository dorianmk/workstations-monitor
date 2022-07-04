
namespace Database.Interfaces.Migration
{
    public interface IMigrations
    {
        int GetLastMigrationNumber();
        void Seed();
        void Update();
    }
}
