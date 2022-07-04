
namespace Common.Interfaces
{
    public interface IAction<T>
    {
        void Do(T item);
    }
}
