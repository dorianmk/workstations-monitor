using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Events;
using AdminClientApp.ViewModels.Essential.Workstations;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.RequestResponse;
using Common.Enums;
using Common.Interfaces;
using DataTransfer.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace AdminClientApp.ViewModels.Essential
{
    public class EventsViewModel : BindableBase
    {
        public IReadOnlyList<DateTimeFilterMode> DateTimeFilterModes { get; }
        public RelayCommand ViewLoadedCmd { get; }
        public RelayCommand WorkstationsSelectionChangedCmd { get; }
        public RelayCommand RefreshCmd { get; }
        public RelayCommand SaveToCsvCmd { get; }
        public ICollectionView EventsView { get; set; }
        public EventsSettingsViewModel EventsSettingsVM { get; }
        private EventsProvider EventsProvider { get; }
        private IProvider<WorkstationViewModel> WorkstationsProvider { get; }
        private List<WorkstationViewModel> SelectedWorkstations { get; set; }

        internal EventsViewModel(
            IProvider<WorkstationViewModel> workstationsProvider,
            IClientConnection connection,
            IFactory<EventDTO, EventViewModel> eventViewModelFactory,
            IFactory<EventRuleViewModel, EventRuleDTO> eventRuleToDtoConverter,
            IFactory<EventRuleDTO, EventRuleViewModel> eventRuleToViewModelConverter,
            IRequestResponse requestResponse)
        {
            WorkstationsProvider = workstationsProvider;
            EventsProvider = new EventsProvider(eventViewModelFactory, requestResponse);
            DateTimeFilterModes = Enum.GetValues(typeof(DateTimeFilterMode)).Cast<DateTimeFilterMode>().ToList();
            ViewLoadedCmd = new RelayCommand(ViewLoaded);
            WorkstationsSelectionChangedCmd = new RelayCommand(WorkstationsSelectionChanged);
            RefreshCmd = new RelayCommand(Refresh);
            SaveToCsvCmd = new RelayCommand(SaveToCsv);
            EventsSettingsVM = new EventsSettingsViewModel(connection, eventRuleToDtoConverter, eventRuleToViewModelConverter, requestResponse);
        }

        private void ViewLoaded(object obj)
        {
            EventsView = CollectionViewSource.GetDefaultView(new List<EventViewModel>());
            AllWorkstations = WorkstationsProvider.Items.ToList();
            SelectedWorkstations = AllWorkstations.ToList();
            ShowInformations = true;
            ShowWarnings = true;
            ShowErrors = true;
            DescriptionFilter = string.Empty;
            if (SelectedDateTimeFilter == DateTimeFilterMode.LastHour)
                RefreshView();
            else
                SelectedDateTimeFilter = DateTimeFilterMode.LastHour;
        }

        private void WorkstationsSelectionChanged(object obj)
        {
            var args = obj as SelectionChangedEventArgs;
            foreach (var item in args.RemovedItems)
                SelectedWorkstations.Remove(item as WorkstationViewModel);
            foreach (var item in args.AddedItems)
                SelectedWorkstations.Add(item as WorkstationViewModel);
            RefreshFilter();
        }

        private void Refresh(object obj)
        {
            RefreshView();
        }

        private void SaveToCsv(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv) | *.csv";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    using (TextWriter tw = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var item in EventsView)
                        {
                            var vm = item as EventViewModel;
                            tw.WriteLine($"{vm.DateTime};{vm.WorkstationName};{vm.EventType};{vm.Description}");
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private IReadOnlyList<WorkstationViewModel> allWorkstations;
        public IReadOnlyList<WorkstationViewModel> AllWorkstations
        {
            get => allWorkstations;
            private set => SetProperty(ref allWorkstations, value);
        }

        private DateTimeFilterMode selectedDateTimeFilter;
        public DateTimeFilterMode SelectedDateTimeFilter
        {
            get => selectedDateTimeFilter;
            set
            {
                if (SetProperty(ref selectedDateTimeFilter, value))
                    RefreshView();
            }
        }

        private bool showInformations;
        public bool ShowInformations
        {
            get => showInformations;
            set
            {
                if (SetProperty(ref showInformations, value))
                    RefreshFilter();
            }
        }

        private bool showWarnings;
        public bool ShowWarnings
        {
            get => showWarnings;
            set
            {
                if (SetProperty(ref showWarnings, value))
                    RefreshFilter();
            }
        }

        private bool showErrors;
        public bool ShowErrors
        {
            get => showErrors;
            set
            {
                if (SetProperty(ref showErrors, value))
                    RefreshFilter();
            }
        }

        private string descriptionFilter;
        public string DescriptionFilter
        {
            get => descriptionFilter;
            set
            {
                if (SetProperty(ref descriptionFilter, value))
                    RefreshFilter();
            }
        }

        public DateTime LastUpdateDateTime => DateTime.Now;

        private void RefreshView()
        {
            var events = EventsProvider.Get(SelectedDateTimeFilter);
            EventsView = CollectionViewSource.GetDefaultView(events);
            EventsView.Filter = Filter;
            OnPropertyChanged(nameof(EventsView));
            OnPropertyChanged(nameof(LastUpdateDateTime));
        }

        private void RefreshFilter() => EventsView.Refresh();

        private bool Filter(object obj) => obj is EventViewModel @event && FilterType(@event) && FilterWorkstation(@event) && FilterDescription(@event);

        private bool FilterType(EventViewModel @event)
        {
            switch (@event.EventType)
            {
                case EventType.Information: return ShowInformations;
                case EventType.Warning: return ShowWarnings;
                case EventType.Error: return ShowErrors;
                default: throw new NotImplementedException();
            }
        }

        private bool FilterWorkstation(EventViewModel @event)
        {
            return SelectedWorkstations.Any(x => x.Id == @event.WorkstationId);
        }

        private bool FilterDescription(EventViewModel @event)
        {
            if (string.IsNullOrEmpty(DescriptionFilter))
                return true;
            else
                return @event.Description.ToLowerInvariant().Contains(DescriptionFilter.ToLowerInvariant());
        }

    }
}
