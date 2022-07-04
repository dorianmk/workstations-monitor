using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using DataTransfer.Interfaces;
using Diagnostics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkstationService.Essential.ProcessesInfo
{
    internal interface IProcessesInfoWorker : IWorker
    {
    }

    internal class ProcessesInfoWorker : IProcessesInfoWorker
    {
        private IDiagnostics Diagnostics { get; }
        private IFactory<IProcessInfo, ProcessInfoDTO> ProcessInfoDtoFactory { get; }
        private IBuffer<IData> DataPacketsSender { get; }

        public ProcessesInfoWorker(IDiagnostics diagnostics, IFactory<IProcessInfo, ProcessInfoDTO> processInfoDtoFactory, IBuffer<IData> dataPacketsSender)
        {
            Diagnostics = diagnostics;
            ProcessInfoDtoFactory = processInfoDtoFactory;
            DataPacketsSender = dataPacketsSender;
        }

        public void Start()
        {
            Diagnostics.ProcessesInfoProvider.Actual += ProcessesInfoProvider_Actual;
        }

        public void Stop()
        {
            Diagnostics.ProcessesInfoProvider.Actual -= ProcessesInfoProvider_Actual;
        }

        private void ProcessesInfoProvider_Actual(object sender, IEnumerable<IProcessInfo> processes)
        {
            var packet = new ProcessesInfoPacket(DateTime.Now);
            packet.ProcessesInfo = processes.Select(x => ProcessInfoDtoFactory.Create(x)).ToList();
            DataPacketsSender.Add(packet);
        }
    }
}
