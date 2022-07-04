using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using DataTransfer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Workstations
{   
    internal class WorkstationsProvider : IProvider<WorkstationViewModel>
    {
        private List<WorkstationViewModel> List { get; }
        private IClientConnection Connection { get; }
        private IUpserter<WorkstationDTO, WorkstationViewModel> WorkstationViewModelUpserter { get; }

        public event EventHandler<WorkstationViewModel> ItemAdded;
        public IReadOnlyCollection<WorkstationViewModel> Items => List.AsReadOnly();

        public WorkstationsProvider(IClientConnection connection, IUpserter<WorkstationDTO, WorkstationViewModel> workstationViewModelUpserter)
        {
            Connection = connection;
            List = new List<WorkstationViewModel>();
            WorkstationViewModelUpserter = workstationViewModelUpserter;
            Connection.Server.OnRead += OnDataRead;
        }

        private void OnDataRead(object sender, IData readData)
        {
            if (readData is WorkstationDTO workstationDTO)
            {
                var found = Items.FirstOrDefault(x => x.Id.Equals(workstationDTO.Id));
                if (found != null)
                    WorkstationViewModelUpserter.Update(workstationDTO, found);
                else
                {
                    var item = WorkstationViewModelUpserter.Create(workstationDTO);
                    List.Add(item);
                    ItemAdded?.Invoke(this, item);
                }
            }
        }
    }

}
