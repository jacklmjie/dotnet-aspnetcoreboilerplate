using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// TODO:主键和自增未实现
        /// </summary>
        public readonly DbContext _dbContext;
        public readonly DbSet<T> _dbSet;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public virtual int Count()
        {
            return _dbSet.Count();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        public virtual int Add(T entity, bool IsCommit = false)
        {
            _dbSet.Add(entity);
            int i_flag = IsCommit ? _dbContext.SaveChanges() : 0;
            return i_flag;
        }

        public virtual int Update(T entity, bool IsCommit = false)
        {
            var entry = _dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
            _dbSet.Attach(entity);
            int i_flag = IsCommit ? _dbContext.SaveChanges() : 0;
            return i_flag;
        }

        public virtual int Update(T entity, bool IsCommit = false, params string[] proName)
        {
            var entry = _dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
            foreach (string s in proName)
            {
                entry.Property(s).IsModified = true;
            }
            _dbSet.Attach(entity);
            int i_flag = IsCommit ? _dbContext.SaveChanges() : 0;
            return i_flag;
        }

        public virtual int Delete(T entity, bool IsCommit = false)
        {
            T existing = _dbSet.Find(entity);
            if (existing != null) _dbSet.Remove(existing);
            int i_flag = IsCommit ? _dbContext.SaveChanges() : 0;
            return i_flag;
        }

        public virtual int Delete(Expression<Func<T, bool>> predicate, bool IsCommit = false)
        {
            var enties = _dbSet.Where(predicate).ToList();
            foreach (var item in enties)
            {
                _dbSet.Remove(item);
            }
            int i_flag = IsCommit ? _dbContext.SaveChanges() : 0;
            return i_flag;
        }
    }
}
