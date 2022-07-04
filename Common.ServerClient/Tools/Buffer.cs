using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Tools
{
    public class Buffer<T> : IBuffer<T>
    {
        private IAction<T> Action { get; }
        private ManualResetEvent PauseResetEvent { get; }
        private BlockingCollection<T> Collection { get; }
        private Task ConsumerTask { get; }
        private bool IsPaused { get; set; }

        public Buffer(IAction<T> action)
        {
            Action = action;
            IsPaused = true;
            PauseResetEvent = new ManualResetEvent(false);
            Collection = new BlockingCollection<T>();
            ConsumerTask = new Task(Consume, TaskCreationOptions.LongRunning);
            ConsumerTask.Start();
        }

        public void Add(T item)
        {
            Collection.Add(item);
        }

        public void Start()
        {
            IsPaused = false;
            PauseResetEvent.Set();
        }

        public void Stop()
        {
            IsPaused = true;
            PauseResetEvent.Reset();
        }

        private void Consume(object obj)
        {
            foreach (var item in Collection.GetConsumingEnumerable())
            {
                if (IsPaused)
                    PauseResetEvent.WaitOne();
                Action.Do(item);
            }
        }
    }
}
