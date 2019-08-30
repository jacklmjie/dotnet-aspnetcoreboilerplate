using System;

namespace Core.Repository.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
