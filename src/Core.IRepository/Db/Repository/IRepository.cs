using System;
using System.Linq.Expressions;

namespace Core.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> predicate);

        int Count();

        int Count(Expression<Func<T, bool>> predicate);

        int Add(T entity, bool IsCommit = false);

        int Update(T entity, bool IsCommit = false);

        int Update(T entity, bool IsCommit = false, params string[] proName);

        int Delete(T entity, bool IsCommit = false);

        int Delete(Expression<Func<T, bool>> predicate, bool IsCommit = false);
    }
}
