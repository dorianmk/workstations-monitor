using System;
using System.Collections.Generic;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Database.Interfaces;
using Database.Interfaces.Workstation;
using DataTransfer.Interfaces;

namespace ServerService.Essential.Workstation
{
    internal interface IWorkstationsManager
    {
        IReadOnlyDictionary<string, IWorkstationCache> Workstations { get; }
        void OnNewClient(IConnection clientSource);

        event EventHandler<IWorkstationCache> WorkstationChanged;

        event EventHandler<IData> WorkstationDataRead;
    }

    internal class WorkstationsManager : IWorkstationsManager
    {
        private IDatabase Database { get; }
        private IFactory<IWorkstation, IWorkstationCache> Factory { get; }
        private Dictionary<string, IWorkstationCache> Collection { get; }

        public IReadOnlyDictionary<string, IWorkstationCache> Workstations => Collection;

        public event EventHandler<IWorkstationCache> WorkstationChanged;

        public event EventHandler<IData> WorkstationDataRead;

        public WorkstationsManager(IDatabase database, IFactory<IWorkstation, IWorkstationCache> factory)
        {
            Database = database;
            Factory = factory;
            Collection = new Dictionary<string, IWorkstationCache>();
            foreach (var workstation in Database.Workstations.GetAll())
            {
                var workstationCache = Factory.Create(workstation);
                Collection.Add(workstation.GetId(), workstationCache);
            }
        }

        public void OnNewClient(IConnection client)
        {
            client.OnRead += OnDataRead;
        }

        private void OnDataRead(object source, IData readData)
        {
            var client = source as IConnection;
            if (readData is ConnectedPacket connected)
            {
                var workstationId = connected.WorkstationId;

                if (workstationId == null)
                {
                    var workstation = Database.Workstations.AddWorkstation(connected.ComputerName);
                    var newWorkstationCache = Factory.Create(workstation);
                    workstationId = workstation.GetId();
                    Collection.Add(workstationId, newWorkstationCache);
                }

                var workstationCache = Collection[workstationId];
                client.OnRead += (s, e) => WorkstationDataRead?.Invoke(workstationCache, e);               
                workstationCache.SetConnect(client);
                client.Stopped += (s, e) => WorkstationChanged?.Invoke(this, workstationCache);
                WorkstationChanged?.Invoke(this, workstationCache);
            }
            else
            {
                client.OnRead -= OnDataRead;
            }
        }
    }
}
