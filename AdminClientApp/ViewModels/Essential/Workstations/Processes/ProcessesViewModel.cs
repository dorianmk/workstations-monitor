using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using AdminClientApp.Entry;
using AdminClientApp.ViewModels.Common;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Workstations.Processes
{
    public class ProcessesViewModel : BindableBase
    {
        private IUpserter<ProcessInfoDTO, ProcessInfoViewModel> ProcessInfoViewModelUpserter { get; }
        private ObservableCollection<ProcessInfoViewModel> ProcessesCollection { get; }
        
        public ListCollectionView Processes { get; private set; }

        internal ProcessesViewModel(IUpserter<ProcessInfoDTO, ProcessInfoViewModel> processInfoViewModelUpserter)
        {
            ProcessInfoViewModelUpserter = processInfoViewModelUpserter;
            ProcessesCollection = new ObservableCollection<ProcessInfoViewModel>();
            App.Current.Dispatcher.Invoke(() => Processes = new ListCollectionView(ProcessesCollection));
            Processes.IsLiveSorting = true;
            Processes.SortDescriptions.Add(new SortDescription(nameof(ProcessInfoViewModel.CPUPercent), ListSortDirection.Descending));
        }

        internal void Update(ProcessesInfoPacket processesInfoPacket)
        {
            AddOrUpdate(processesInfoPacket.ProcessesInfo);
            Remove(processesInfoPacket.ProcessesInfo);
        }

        private void AddOrUpdate(List<ProcessInfoDTO> processInfoDTOs)
        {
            foreach (var dto in processInfoDTOs)
            {
                var found = ProcessesCollection.FirstOrDefault(x => x.Name.Equals(dto.Name));
                if (found == null)
                {
                    var item = ProcessInfoViewModelUpserter.Create(dto);
                    App.Current.Dispatcher.Invoke(() => ProcessesCollection.Add(item));
                }
                else
                {
                    ProcessInfoViewModelUpserter.Update(dto, found);
                    found.Notify();
                }
            }
        }

        private void Remove(List<ProcessInfoDTO> processInfoDTOs)
        {
            var processesIndexesToRemove = new List<int>();
            var names = new HashSet<string>(processInfoDTOs.Select(x => x.Name));
            for (int i = ProcessesCollection.Count - 1; i >= 0; i--)
            {
                if (!names.Contains(ProcessesCollection[i].Name))
                    processesIndexesToRemove.Add(i);
            }
            foreach (var index in processesIndexesToRemove)
                App.Current.Dispatcher.Invoke(() => ProcessesCollection.RemoveAt(index));
        }
    }
}
