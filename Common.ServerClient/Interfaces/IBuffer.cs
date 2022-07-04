
namespace Common.Interfaces
{
    public interface IBuffer<T> : IWorker
    {
        void Add(T item);
    }
}
