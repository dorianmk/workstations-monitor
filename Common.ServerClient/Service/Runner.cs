using System;
using System.ServiceProcess;
using Common.Interfaces;

namespace Common.Service
{
    public class Runner : IServiceRunner
    {
        private IWorker Worker { get; }
        private IFactory<ServiceBase> ServiceFactory { get; }

        public Runner(IWorker worker, IFactory<ServiceBase> serviceFactory)
        {
            Worker = worker;
            ServiceFactory = serviceFactory;
        }

        public void Run()
        {
            if (!Environment.UserInteractive)
            {
                using (var service = ServiceFactory.Create())
                    ServiceBase.Run(service);
            }
            else
            {
                Worker.Start();
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
                Worker.Stop();
                Console.WriteLine("Stopped. Press any key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
