using System;

namespace Core.Repository.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
