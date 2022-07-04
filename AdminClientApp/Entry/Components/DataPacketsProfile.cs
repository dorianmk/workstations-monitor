using AutoMapper;
using Common.DataTransfer.DataPackets.Workstation;
using Common.DataTransfer.DataPackets.AdminClient;
using AdminClientApp.ViewModels.Essential.Workstations;
using AdminClientApp.ViewModels.Essential.Workstations.Processes;

namespace AdminClientApp.Entry.Components
{
    internal class DataPacketsProfile : Profile
    {        
        public DataPacketsProfile()
        {
            CreateMap<WorkstationDTO, WorkstationViewModel>().ConstructUsingServiceLocator();
            CreateMap<ProcessInfoDTO, ProcessInfoViewModel>();          
        }
    }
}
