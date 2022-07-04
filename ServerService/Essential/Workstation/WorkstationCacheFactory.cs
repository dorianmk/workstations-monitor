using Common.Interfaces;
using Database.Interfaces.Workstation;

namespace ServerService.Essential.Workstation
{
    internal class WorkstationCacheFactory : IFactory<IWorkstation, IWorkstationCache>
    {
        public WorkstationCacheFactory()
        {
        }

        public IWorkstationCache Create(IWorkstation param)
        {
            return new WorkstationCache(param);
        }
    }
}
