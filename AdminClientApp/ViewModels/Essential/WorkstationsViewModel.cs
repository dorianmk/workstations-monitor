using AdminClientApp.Entry;
using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Workstations;
using Common.Interfaces;
using System.Collections.ObjectModel;

namespace AdminClientApp.ViewModels.Essential
{
    public class WorkstationsViewModel : BindableBase
    {
        private IProvider<WorkstationViewModel> WorkstationsProvider { get; }

        public ObservableCollection<WorkstationViewModel> Workstations { get; }

        private WorkstationViewModel selectedWorkstation;
        public WorkstationViewModel SelectedWorkstation
        {
            get => selectedWorkstation;
            set => SetProperty(ref selectedWorkstation, value);
        }

        internal WorkstationsViewModel(IProvider<WorkstationViewModel> workstationsProvider)
        {
            WorkstationsProvider = workstationsProvider;
            Workstations = new ObservableCollection<WorkstationViewModel>(WorkstationsProvider.Items);
            WorkstationsProvider.ItemAdded += OnItemAdded;
        }

        private void OnItemAdded(object sender, WorkstationViewModel item)
        {
            App.Current.Dispatcher.Invoke(() => Workstations.Add(item));
        }

    }
}
