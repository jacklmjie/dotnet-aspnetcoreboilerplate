using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.IContract
{
    public interface IIdentityContract
    {
        Task<User> GetUserByName(string userName);
    }
}
