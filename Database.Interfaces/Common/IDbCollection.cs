using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Database.Interfaces.Common
{
    public interface IDbCollection<T>
        where T : IEntity
    {
        List<T> GetAll();
        List<T> FindAll(Expression<Func<T, bool>> predicate);
        T FindFirst(Expression<Func<T, bool>> predicate);
        T GetOne(string id);
        bool AddOrReplace(T entity);
        bool Remove(string id);
        void ReplaceCollection(IEnumerable<T> items);
    }
}
