using System.Configuration;
using Database.MongoDB;

namespace ServerService.Entry.Settings
{
    internal class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; }

        public DatabaseSettings()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
        }
    }
}
