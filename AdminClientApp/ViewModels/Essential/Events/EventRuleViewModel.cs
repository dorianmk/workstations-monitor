using AdminClientApp.ViewModels.Common;
using Common.Enums;
using System;

namespace AdminClientApp.ViewModels.Essential.Events
{
    public class EventRuleViewModel : BindableBase
    {

        internal EventRuleViewModel()
        {
            Value = GetDefaultValue(EventSource);
        }

        private EventSource eventSource;
        public EventSource EventSource
        {
            get => eventSource;
            set
            {
                if (SetProperty(ref eventSource, value))
                {
                    OnPropertyChanged(nameof(Unit));
                    Value = GetDefaultValue(EventSource);
                    OnPropertyChanged(nameof(Minimum));
                    OnPropertyChanged(nameof(Maximum));
                }
            }
        }

        public string Unit
        {
            get
            {
                switch (EventSource)
                {
                    case EventSource.CPU: return "%";
                    case EventSource.Memory: return "MB";
                    case EventSource.TCP: return "KB";
                    case EventSource.Disk: return "KB";
                    default: throw new NotImplementedException();
                }
            }
        }

        private ComparisonMode comparisonMode;
        public ComparisonMode ComparisonMode
        {
            get => comparisonMode;
            set => SetProperty(ref comparisonMode, value);
        }

        private int _value;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private static int GetDefaultValue(EventSource eventSource)
        {
            switch (eventSource)
            {
                case EventSource.CPU: return 50;
                case EventSource.Memory: return 4096;
                case EventSource.TCP: return 10000;
                case EventSource.Disk: return 10000;
                default: throw new NotImplementedException();
            }
        }

        public int Minimum => 0;

        public int Maximum
        {
            get
            {
                switch (EventSource)
                {
                    case EventSource.CPU: return 100;
                    case EventSource.Memory: return int.MaxValue;
                    case EventSource.TCP: return int.MaxValue;
                    case EventSource.Disk: return int.MaxValue;
                    default: throw new NotImplementedException();
                }
            }
        }

        private EventType eventType;
        public EventType EventType
        {
            get => eventType;
            set => SetProperty(ref eventType, value);
        }

    }

}
