using System.Configuration;

namespace WorkstationService.Essential.Id
{
    internal interface IWorkstationId
    {
        string Get();
        void Set(string id);
    }

    internal class WorkstationId : IWorkstationId
    {
        private string Key { get; }

        public string Get() => ConfigurationManager.AppSettings[Key]?.ToString();

        public void Set(string id)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(Key);
            config.AppSettings.Settings.Add(Key, id);
            config.Save();
        }

        public WorkstationId()
        {
            Key = "id";
        }
    }
}
