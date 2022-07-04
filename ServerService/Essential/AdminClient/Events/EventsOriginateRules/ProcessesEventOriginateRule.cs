using Common.DataTransfer.DataPackets.Workstation;
using Common.Enums;
using System;
using System.Linq;

namespace ServerService.Essential.AdminClient.Events.EventsOriginateRules
{
    internal abstract class ProcessesEventOriginateRule : IEventOriginateRule<ProcessesInfoPacket>
    {
        internal int Value { get; }
        internal ComparisonMode ComparisonMode { get; }
        internal EventType EventType { get; }
        internal EventSource EventSource { get; }

        protected ProcessesEventOriginateRule(int value, ComparisonMode comparisonMode, EventType eventType, EventSource eventSource)
        {
            Value = value;
            ComparisonMode = comparisonMode;
            EventType = eventType;
            EventSource = eventSource;
        }

        public bool IsFulfilled(ProcessesInfoPacket processesInfo, out NewEvent newEvent)
        {
            var actualValue = processesInfo.ProcessesInfo.Sum(x => GetValue(x));
            if (InvokeEvent(actualValue))
            {
                var description = GetEventDescription(processesInfo, actualValue);
                newEvent = new NewEvent(processesInfo.DateTime, EventType, processesInfo.WorkstationId, description);
                return true;
            }
            else
            {
                newEvent = null;
                return false;
            }
        }

        private string GetEventDescription(ProcessesInfoPacket processesInfo, double actualValue)
        {
            var count = 3;
            var topProcesses = processesInfo.ProcessesInfo.OrderByDescending(x => GetValue(x)).Take(count).ToArray();
            var topProcessesDescription = string.Join(", ", Enumerable.Range(1, count).Select(i => $"{i}.{topProcesses[i - 1].Name} {GetValue(topProcesses[i - 1])}{Unit}"));
            return $"{EventSource} usage is {actualValue}{Unit} ({topProcessesDescription}...)";
        }

        private bool InvokeEvent(double actualValue)
        {
            switch (ComparisonMode)
            {
                case ComparisonMode.Greater: return actualValue > Value;
                case ComparisonMode.GreaterOrEqual: return actualValue >= Value;
                case ComparisonMode.Less: return actualValue < Value;
                case ComparisonMode.LessOrEqual: return actualValue <= Value;
                default: throw new NotImplementedException();
            }
        }

        protected abstract double GetValue(ProcessInfoDTO processInfo);
        protected abstract string Unit { get; }
    }
}
