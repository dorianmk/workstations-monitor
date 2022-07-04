
namespace Common.Interfaces
{
    public interface IUpserter<TSource, TTarget> : IFactory<TSource, TTarget>, IUpdater<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
    }
}
