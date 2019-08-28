namespace Core.Repository.Infrastructure.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
