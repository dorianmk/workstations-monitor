using System.ServiceProcess;
using Common.Interfaces;

namespace WorkstationService.Entry.Service
{
    internal class ServiceFactory : IFactory<ServiceBase>
    {
        private IWorker MainWorker { get; }

        public ServiceFactory(IWorker mainWorker)
        {
            MainWorker = mainWorker;
        }

        public ServiceBase Create()
        {
            return new Service(MainWorker, "WorkstationService");
        }
    }
}
