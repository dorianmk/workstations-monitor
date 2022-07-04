using AdminClientApp.ViewModels.Common;
using Common.DataTransfer.DataPackets.Workstation;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Workstations.Performance
{
    public class PerformanceViewModel : BindableBase
    {
        private double cpu;
        private double memory;
        private double tcpSent;
        private double tcpReceived;
        private double diskIORead;
        private double diskIOWrite;
        private uint seconds = 60;
        public uint MaxSeconds => 600;

        private List<DateTimePoint> CpuAll { get; }
        private List<DateTimePoint> MemoryAll { get; }
        private List<DateTimePoint> TcpSentAll { get; }
        private List<DateTimePoint> TcpReceivedAll { get; }
        private List<DateTimePoint> DiskIOReadAll { get; }
        private List<DateTimePoint> DiskIOWriteAll { get; }
        public ChartValues<DateTimePoint> CPUSeries { get; }
        public ChartValues<DateTimePoint> MemorySeries { get; }
        public ChartValues<DateTimePoint> TcpSentSeries { get; }
        public ChartValues<DateTimePoint> TcpReceivedSeries { get; }
        public ChartValues<DateTimePoint> DiskIOReadSeries { get; }
        public ChartValues<DateTimePoint> DiskIOWriteSeries { get; }
        public uint Seconds
        {
            get => seconds;
            set
            {
                if (SetProperty(ref seconds, value))
                {
                    RefilterSeries(CpuAll, CPUSeries);
                    RefilterSeries(MemoryAll, MemorySeries);
                    RefilterSeries(TcpSentAll, TcpSentSeries);
                    RefilterSeries(TcpReceivedAll, TcpReceivedSeries);
                    RefilterSeries(DiskIOReadAll, DiskIOReadSeries);
                    RefilterSeries(DiskIOWriteAll, DiskIOWriteSeries);
                }
            }
        }

        public double CPU
        {
            get => cpu;
            private set => SetProperty(ref cpu, value);
        }
        public double Memory
        {
            get => memory;
            private set => SetProperty(ref memory, value);
        }
        public double TcpSent
        {
            get => tcpSent;
            private set => SetProperty(ref tcpSent, value);
        }
        public double TcpReceived
        {
            get => tcpReceived;
            private set => SetProperty(ref tcpReceived, value);
        }
        public double DiskIORead
        {
            get => diskIORead;
            private set => SetProperty(ref diskIORead, value);
        }
        public double DiskIOWrite
        {
            get => diskIOWrite;
            private set => SetProperty(ref diskIOWrite, value);
        }

        internal PerformanceViewModel()
        {
            CpuAll = new List<DateTimePoint>();
            MemoryAll = new List<DateTimePoint>();
            TcpSentAll = new List<DateTimePoint>();
            TcpReceivedAll = new List<DateTimePoint>();
            DiskIOReadAll = new List<DateTimePoint>();
            DiskIOWriteAll = new List<DateTimePoint>();
            CPUSeries = new ChartValues<DateTimePoint>();
            MemorySeries = new ChartValues<DateTimePoint>();
            TcpSentSeries = new ChartValues<DateTimePoint>();
            TcpReceivedSeries = new ChartValues<DateTimePoint>();
            DiskIOReadSeries = new ChartValues<DateTimePoint>();
            DiskIOWriteSeries = new ChartValues<DateTimePoint>();
        }

        internal void Update(ProcessesInfoPacket processesInfoPacket)
        {
            CPU = processesInfoPacket.ProcessesInfo.Sum(x => x.CPUPercent);
            Memory = processesInfoPacket.ProcessesInfo.Sum(x => x.MemoryMB);
            TcpSent = processesInfoPacket.ProcessesInfo.Sum(x => x.TcpSentKB);
            TcpReceived = processesInfoPacket.ProcessesInfo.Sum(x => x.TcpReceivedKB);
            DiskIORead = processesInfoPacket.ProcessesInfo.Sum(x => x.DiskIOReadKB);
            DiskIOWrite = processesInfoPacket.ProcessesInfo.Sum(x => x.DiskIOWriteKB);
            UpdateSeries(CPU, CpuAll, CPUSeries);
            UpdateSeries(Memory, MemoryAll, MemorySeries);
            UpdateSeries(TcpSent, TcpSentAll, TcpSentSeries);
            UpdateSeries(TcpReceived, TcpReceivedAll, TcpReceivedSeries);
            UpdateSeries(DiskIORead, DiskIOReadAll, DiskIOReadSeries);
            UpdateSeries(DiskIOWrite, DiskIOWriteAll, DiskIOWriteSeries);
        }

        private void UpdateSeries(double value, List<DateTimePoint> all, ChartValues<DateTimePoint> series)
        {
            var date = DateTime.Now;
            all.Add(new DateTimePoint(date, value));
            series.Add(all.Last());
            if ((date - series.First().DateTime).TotalSeconds > Seconds)
                series.RemoveAt(0);
        }

        private void RefilterSeries(List<DateTimePoint> all, ChartValues<DateTimePoint> series)
        {
            series.Clear();
            series.AddRange(all.Where(x => (DateTime.Now - x.DateTime).TotalSeconds <= Seconds));
        }

        public Func<double, string> XFormatter { get; } = val => new DateTime((long)val).ToString("H:mm:ss");

        public Func<double, string> YPercentFormatter { get; } = val => $"{val} %";
        public Func<double, string> YMegaBytesFormatter { get; } = val => $"{val} MB";
        public Func<double, string> YKiloBytesFormatter { get; } = val => $"{val} KB";
    }
}
