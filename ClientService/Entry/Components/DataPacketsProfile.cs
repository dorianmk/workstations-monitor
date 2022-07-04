using AutoMapper;
using Common.DataTransfer.DataPackets.Workstation;
using Diagnostics.Interfaces;

namespace WorkstationService.Entry.Components
{
    internal class DataPacketsProfile : Profile
    {
        public DataPacketsProfile()
        {
            CreateMap<IProcessInfo, ProcessInfoDTO>();
            CreateMap<ISystemInfoProvider, SystemInfoDTO>();
        }
    }
}
