using AutoMapper;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Workstations
{
    internal class WorkstationViewModelUpserter : IUpserter<WorkstationDTO, WorkstationViewModel>
    {
        private IMapper Mapper { get; }

        public WorkstationViewModelUpserter(IMapper mapper)
        {
            Mapper = mapper;
        }

        public WorkstationViewModel Create(WorkstationDTO param)
        {
            var result = Mapper.Map<WorkstationViewModel>(param);
            return result;
        }

        public WorkstationViewModel Update(WorkstationDTO from, WorkstationViewModel to)
        {
            return Mapper.Map(from, to);
        }
    }
}
