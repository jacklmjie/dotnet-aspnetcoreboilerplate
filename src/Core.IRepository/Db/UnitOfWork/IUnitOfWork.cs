namespace Core.IRepository
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
