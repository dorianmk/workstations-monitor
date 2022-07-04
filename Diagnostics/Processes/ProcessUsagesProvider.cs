using Diagnostics.Processes.ETW;
using System;
using System.Diagnostics;

namespace Diagnostics.Processes
{
    internal class ProcessUsagesProvider
    {
        private Process Process { get; }
        private ETWCounters ETWCounters { get; }
        private Stopwatch Stopwatch { get; }
        private int ProcessorCores { get; }

        private TimeSpan oldCPUTime;
        private long oldTimestamp;
        private bool isValid;

        internal ProcessUsagesProvider(Process process, IETWEvents eTWEvents, int processorCores)
        {
            Process = process;
            ETWCounters = new ETWCounters(Process.Id, eTWEvents);
            ProcessorCores = processorCores;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            isValid = TryGetTotalProcessorTime(out oldCPUTime, out oldTimestamp);
        }

        internal double GetCPU()
        {
            if (isValid)
            {
                TimeSpan newCPUTime;
                long newTimestamp;
                isValid = TryGetTotalProcessorTime(out newCPUTime, out newTimestamp);
                if (isValid)
                {
                    var result = ((newCPUTime.Ticks - oldCPUTime.Ticks) * 100.0) / ((newTimestamp - oldTimestamp) * Environment.ProcessorCount * ProcessorCores);
                    oldCPUTime = newCPUTime;
                    oldTimestamp = newTimestamp;
                    return result;
                }
            }
            return 0;
        }

        internal double GetMemory()
        {
            double result = 0;
            try
            {
                result = Process.PrivateMemorySize64 / 1048576.0;
            }
            catch
            {
                isValid = false;
            }
            return result;
        }

        internal int GetTcpReceived() => ETWCounters.TcpReceived;

        internal int GetTcpSent() => ETWCounters.TcpSent;

        internal int GetDiskIORead() => ETWCounters.DiskIORead;

        internal int GetDiskIOWrite() => ETWCounters.DiskIOWrite;

        private bool TryGetTotalProcessorTime(out TimeSpan result, out long timestamp)
        {
            try
            {
                result = Process.TotalProcessorTime;
                timestamp = Stopwatch.ElapsedTicks;
                return true;
            }
            catch
            {
                result = TimeSpan.Zero;
                timestamp = Stopwatch.ElapsedTicks;
                return false;
            }
        }

    }
}
