using AutoMapper;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Diagnostics.Interfaces;

namespace WorkstationService.Essential.SystemInfo
{
    internal class SystemInfoDTOFactory : IFactory<SystemInfoDTO>
    {
        private IMapper Mapper { get; }
        private ISystemInfoProvider SystemInfoProvider { get; }

        public SystemInfoDTOFactory(IMapper mapper, ISystemInfoProvider systemInfoProvider)
        {
            Mapper = mapper;
            SystemInfoProvider = systemInfoProvider;
        }

        public SystemInfoDTO Create()
        {
            var systemInfo = SystemInfoProvider;
            return Mapper.Map<SystemInfoDTO>(systemInfo);
        }
    }
}
