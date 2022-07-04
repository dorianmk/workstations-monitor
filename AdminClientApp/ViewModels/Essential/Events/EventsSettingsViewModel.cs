using AdminClientApp.ViewModels.Common;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.RequestResponse;
using Common.Enums;
using Common.Interfaces;
using DataTransfer.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Events
{
    public class EventsSettingsViewModel : BindableBase
    {
        private IClientConnection Connection { get; }
        private IFactory<EventRuleViewModel, EventRuleDTO> EventRuleToDtoConverter { get; }
        private IFactory<EventRuleDTO, EventRuleViewModel> EventRuleToViewModelConverter { get; }
        private IRequestResponse RequestResponse { get; }
        public IReadOnlyList<EventSource> EventSources { get; }
        public IReadOnlyList<ComparisonMode> ComparisonModes { get; }
        public IReadOnlyList<EventType> EventTypes { get; }
        public ObservableCollection<EventRuleViewModel> EventRules { get; }
        public RelayCommand ViewLoadedCmd { get; }
        public RelayCommand AddRuleCmd { get; }
        public RelayCommand DeleteRuleCmd { get; }
        public RelayCommand SaveRulesCmd { get; }

        internal EventsSettingsViewModel(
            IClientConnection connection, 
            IFactory<EventRuleViewModel, EventRuleDTO> eventRuleToDtoConverter,
            IFactory<EventRuleDTO, EventRuleViewModel> eventRuleToViewModelConverter,
            IRequestResponse requestResponse)
        {
            Connection = connection;
            EventRuleToDtoConverter = eventRuleToDtoConverter;
            EventRuleToViewModelConverter = eventRuleToViewModelConverter;
            RequestResponse = requestResponse;
            EventSources = Enum.GetValues(typeof(EventSource)).Cast<EventSource>().ToList();
            ComparisonModes = Enum.GetValues(typeof(ComparisonMode)).Cast<ComparisonMode>().ToList();
            EventTypes = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
            EventRules = new ObservableCollection<EventRuleViewModel>();
            ViewLoadedCmd = new RelayCommand(ViewLoaded);
            AddRuleCmd = new RelayCommand(AddRule);
            DeleteRuleCmd = new RelayCommand(DeleteRule);
            SaveRulesCmd = new RelayCommand(SaveRules);
        }

        private void ViewLoaded(object obj)
        {
            EventRules.Clear();
            var request = new GetEventRulesRequest(Guid.NewGuid());
            if (RequestResponse.TryGetResponse(request, out GetEventRulesResponse response, TimeSpan.FromSeconds(1)))
            {
                foreach (var item in response.EventRules)
                    EventRules.Add(EventRuleToViewModelConverter.Create(item));
            }
        }

        private void AddRule(object obj)
        {
            var newRule = new EventRuleViewModel()
            {
                EventType = EventType.Warning
            };
            EventRules.Add(newRule);
        }

        private void DeleteRule(object obj)
        {
            EventRules.Remove(obj as EventRuleViewModel);
        }

        private void SaveRules(object obj)
        {
            var packet = new SaveEventRulesPacket();
            packet.EventRules = EventRules.Select(x => EventRuleToDtoConverter.Create(x)).ToList();
            Connection.Server.Write(packet);
        }
    }

}
