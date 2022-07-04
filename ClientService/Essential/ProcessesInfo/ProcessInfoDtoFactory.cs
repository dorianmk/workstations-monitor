using AutoMapper;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Diagnostics.Interfaces;

namespace WorkstationService.Essential.ProcessesInfo
{
    internal class ProcessInfoDtoFactory : IFactory<IProcessInfo, ProcessInfoDTO>
    {
        private IMapper Mapper { get; }

        public ProcessInfoDtoFactory(IMapper mapper)
        {
            Mapper = mapper;
        }

        public ProcessInfoDTO Create(IProcessInfo param)
        {
            return Mapper.Map<ProcessInfoDTO>(param);
        }
    }
}
