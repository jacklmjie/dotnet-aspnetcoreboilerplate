using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByName(string userName);
    }
}
