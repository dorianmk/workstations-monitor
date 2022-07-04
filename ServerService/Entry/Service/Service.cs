using Common.Interfaces;
using Common.Service;

namespace ServerService.Entry.Service
{
    public class Service : ServiceWithWorker
    {
        internal Service(IWorker worker, string serviceName) : base(worker, serviceName)
        {
        }
    }
}
