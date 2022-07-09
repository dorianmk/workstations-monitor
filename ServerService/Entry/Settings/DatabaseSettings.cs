using Database.MongoDB;
using Microsoft.Extensions.Configuration;

namespace ServerService.Entry.Settings
{
    internal class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; }

        public DatabaseSettings(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("local");
        }
    }
}
