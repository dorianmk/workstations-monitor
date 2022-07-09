using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using DataTransfer.Interfaces;
using System;
using WorkstationService.Essential.Id;
using WorkstationService.Essential.ProcessesInfo;

namespace WorkstationService.Entry
{
    internal class MainWorker : IWorker
    {
        private IClientConnection ClientConnection { get; }
        private IProcessesInfoWorker ProcessesInfoWorker { get; }
        private IFactory<SystemInfoDTO> SystemInfoDtoFactory { get; }
        private IWorkstationId WorkstationId { get; }
        private IFactory<ConnectedPacket> ConnectedPacketFactory { get; }
        private IAction<string> Logger { get; }
        private IBuffer<IData> DataPacketsSender { get; }

        public MainWorker(
            IClientConnection clientConnection,
            IProcessesInfoWorker processesInfoWorker,
            IFactory<SystemInfoDTO> systemInfoDtoFactory,
            IWorkstationId workstationId,
            IFactory<ConnectedPacket> connectedPacketFactory,
            IAction<string> logger,
            IBuffer<IData> dataPacketsSender)
        {
            ClientConnection = clientConnection;
            ProcessesInfoWorker = processesInfoWorker;
            SystemInfoDtoFactory = systemInfoDtoFactory;
            WorkstationId = workstationId;
            ConnectedPacketFactory = connectedPacketFactory;
            Logger = logger;
            DataPacketsSender = dataPacketsSender;
        }

        public void Start()
        {
            ClientConnection.Stopped += ClientConnection_Stopped;
            ClientConnection.Connected += ClientConnection_Connected;           
            ProcessesInfoWorker.Start();
            ClientConnection.Start();
        }

        private void ClientConnection_Connected(object sender, EventArgs e)
        {
            Logger.Do(nameof(ClientConnection_Connected));
            DataPacketsSender.Start();
            ClientConnection.Server.OnRead += OnDataRead;
            ClientConnection.Server.Write(ConnectedPacketFactory.Create());
            ClientConnection.Server.Write(SystemInfoDtoFactory.Create());
        }

        private void OnDataRead(object source, IData data)
        {
            var server = source as IDataTwoWay;
            if (data is ConnectedPacket connected)
            {
                WorkstationId.Set(connected.WorkstationId);
            }
        }

        private void ClientConnection_Stopped(object sender, EventArgs e)
        {
            Logger.Do(nameof(ClientConnection_Stopped));
            DataPacketsSender.Stop();
            ClientConnection.Start();
        }

        public void Stop()
        {
            ClientConnection.Stopped -= ClientConnection_Stopped;
            ClientConnection.Connected -= ClientConnection_Connected;
            ProcessesInfoWorker.Stop();
            DataPacketsSender.Stop();
        }

    }
}
