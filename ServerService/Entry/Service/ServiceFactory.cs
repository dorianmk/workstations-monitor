using System.ServiceProcess;
using Common.Interfaces;

namespace ServerService.Entry.Service
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
            return new Service(MainWorker, "ServerService");
        }
    }
}
