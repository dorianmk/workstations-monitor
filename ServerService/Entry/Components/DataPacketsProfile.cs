using AutoMapper;
using Common.DataTransfer.DataPackets.AdminClient;
using Database.Interfaces.Workstation;
using ServerService.Essential.Workstation;

namespace ServerService.Entry.Components
{
    internal class DataPacketsProfile : Profile
    {
        public DataPacketsProfile()
        {
            CreateMap<IWorkstationCache, WorkstationDTO>().IncludeMembers(x => x.Workstation);
            CreateMap<IWorkstation, WorkstationDTO>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.GetId()));
        }
    }
}
