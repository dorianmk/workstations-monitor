using Common.Enums;
using Common.Interfaces;
using Database.Interfaces.EventRule;
using ServerService.Essential.AdminClient.Events.EventsOriginateRules;
using System;
using System.Collections.Generic;

namespace ServerService.Essential.AdminClient.Events
{
    internal class ProcessesEventOriginateRulesFactory : IFactory<List<ProcessesEventOriginateRule>>
    {
        private IEventRules EventRules { get; }

        public ProcessesEventOriginateRulesFactory(IEventRules eventRules)
        {
            EventRules = eventRules;
        }

        public List<ProcessesEventOriginateRule> Create()
        {
            var result = new List<ProcessesEventOriginateRule>();
            var eventRulesEntities = EventRules.GetAll();
            foreach (var entity in eventRulesEntities)
            {
                if (Enum.TryParse(entity.EventSource, out EventSource eventSource) &&
                    Enum.TryParse(entity.ComparisonMode, out ComparisonMode comparisonMode) &&
                    Enum.TryParse(entity.EventType, out EventType eventType))
                {
                    ProcessesEventOriginateRule rule;
                    switch (eventSource)
                    {
                        case EventSource.CPU:
                            rule = new CPUEventOriginateRule(entity.Value, comparisonMode, eventType);
                            break;
                        case EventSource.Memory:
                            rule = new MemoryEventOriginateRule(entity.Value, comparisonMode, eventType);
                            break;
                        case EventSource.TCP:
                            rule = new TCPEventOriginateRule(entity.Value, comparisonMode, eventType);
                            break;
                        case EventSource.Disk:
                            rule = new DiskEventOriginateRule(entity.Value, comparisonMode, eventType);
                            break;
                        default: continue;
                    }
                    result.Add(rule);
                }
            }
            return result;
        }
    }
}
