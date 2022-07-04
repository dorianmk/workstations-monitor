using Common.Interfaces;
using Common.Service;

namespace WorkstationService.Entry.Service
{
    public class Service : ServiceWithWorker
    {
        internal Service(IWorker worker, string serviceName) : base(worker, serviceName)
        {
        }
    }
}
