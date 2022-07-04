using AutoMapper;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;

namespace ServerService.Essential.Workstation
{
    internal class WorkstationDTOFactory : IFactory<IWorkstationCache, WorkstationDTO>
    {
        private IMapper Mapper { get; }

        public WorkstationDTOFactory(IMapper mapper)
        {
            Mapper = mapper;
        }

        public WorkstationDTO Create(IWorkstationCache param)
        {
            return Mapper.Map<WorkstationDTO>(param);
        }
    }
}
