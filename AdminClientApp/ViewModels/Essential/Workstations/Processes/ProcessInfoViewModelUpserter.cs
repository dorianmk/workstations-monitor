using AutoMapper;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Workstations.Processes
{
    internal class ProcessInfoViewModelUpserter : IUpserter<ProcessInfoDTO, ProcessInfoViewModel>
    {
        private IMapper Mapper { get; }

        public ProcessInfoViewModelUpserter(IMapper mapper)
        {
            Mapper = mapper;
        }

        public ProcessInfoViewModel Create(ProcessInfoDTO param)
        {
            return Mapper.Map<ProcessInfoViewModel>(param);
        }

        public ProcessInfoViewModel Update(ProcessInfoDTO from, ProcessInfoViewModel to)
        {
            return Mapper.Map(from, to);
        }
    }
}
