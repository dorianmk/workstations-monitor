using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Events;
using AdminClientApp.ViewModels.Essential.Maps;
using AdminClientApp.ViewModels.Essential.Users;
using AdminClientApp.ViewModels.Essential.Workstations;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.RequestResponse;
using Common.Interfaces;
using DataTransfer.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AdminClientApp.ViewModels.Essential
{
    public class AdminPanelViewModel : BindableBase
    {
        public WorkstationsViewModel WorkstationsVM { get; }
        public EventsViewModel EventsVM { get; }
        public MapsViewModel MapsVM { get; }
        public UsersViewModel UsersVM { get; }

        private HamburgerMenuItem selectedMenuItem;
        public HamburgerMenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set => SetProperty(ref selectedMenuItem, value);
        }

        public AdminPanelViewModel(
            IClientConnection connection,
            IProvider<WorkstationViewModel> workstationsProvider,
            IFactory<EventDTO, EventViewModel> eventViewModelFactory,
            IFactory<EventRuleViewModel, EventRuleDTO> eventRuleToDtoConverter,
            IFactory<EventRuleDTO, EventRuleViewModel> eventRuleToViewModelConverter,
            IFactory<MapViewModel, MapDTO> mapDTOFactory,
            IFactory<MapDTO, MapViewModel> mapViewModelFactory,
            IFactory<UserDTO, UserViewModel> userViewModelFactory,
            IDialogCoordinator dialogCoordinator,
            IRequestResponse requestResponse)
        {
            WorkstationsVM = new WorkstationsViewModel(workstationsProvider);
            MapsVM = new MapsViewModel(connection, mapDTOFactory, mapViewModelFactory, workstationsProvider, dialogCoordinator);
            UsersVM = new UsersViewModel(connection, userViewModelFactory);
            EventsVM = new EventsViewModel(workstationsProvider, connection, eventViewModelFactory, eventRuleToDtoConverter, eventRuleToViewModelConverter, requestResponse);
        }

    }
}