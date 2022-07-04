using Database.MongoDB;

namespace Database.Tests.MongoDB
{
    internal class DBSettings : IDatabaseSettings
    {
        public string ConnectionString => "mongodb://localhost:27017/pab_test";
    }
}
