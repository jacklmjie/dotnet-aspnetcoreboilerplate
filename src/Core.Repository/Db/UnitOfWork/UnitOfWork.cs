using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Repository
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWork(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int SaveChanges()
        {
            //ef已经实现了UOW
            return _dbContext.SaveChanges();
        }
    }
}
