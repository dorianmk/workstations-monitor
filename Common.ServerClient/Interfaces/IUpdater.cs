
namespace Common.Interfaces
{
    public interface IUpdater<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        TTarget Update(TSource from, TTarget to);
    }
}
