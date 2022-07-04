using DataTransfer.Interfaces;
using System;
using System.Collections.Generic;

namespace Common.DataTransfer.DataPackets.Workstation
{
    public class ProcessesInfoPacket : IData
    {
        public string WorkstationId { get; set; }
        public List<ProcessInfoDTO> ProcessesInfo { get; set; }
        public DateTime DateTime { get; private set; }

        public ProcessesInfoPacket(DateTime dateTime)
        {
            DateTime = dateTime;
        }
    }

    public class ProcessInfoDTO
    {
        public string Name { get; set; }
        public double CPUPercent { get; set; }
        public int MemoryMB { get; set; }
        public int TcpReceivedKB { get; set; }
        public int TcpSentKB { get; set; }
        public int DiskIOReadKB { get; set; }
        public int DiskIOWriteKB { get; set; }
        public string ExecPath { get; set; }
    }
}
