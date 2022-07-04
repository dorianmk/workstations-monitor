using System.ServiceProcess;
using Common.Interfaces;

namespace Common.Service
{
    public class ServiceWithWorker : ServiceBase
    {
        private IWorker Worker { get; }

        protected ServiceWithWorker(IWorker worker, string serviceName) : base()
        {
            ServiceName = serviceName;
            Worker = worker;
        }

        protected override void OnStart(string[] args) => Worker.Start();

        protected override void OnStop() => Worker.Stop();

    }
}
