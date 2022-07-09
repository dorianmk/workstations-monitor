using WorkstationService.Entry.Settings;

namespace WorkstationService.Essential.Id
{
    internal interface IWorkstationId
    {
        string Get();
        void Set(string id);
    }

    internal class WorkstationId : IWorkstationId
    {
        private readonly AppSettings _appSettings;

        public WorkstationId(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string Get() => _appSettings.WorkstationId;

        public void Set(string id)
        {
            _appSettings.WorkstationId = id;
        }
    }
}
