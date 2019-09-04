using Core.Models.Identity.Entities;
using System.Threading.Tasks;

namespace Core.IContract
{
    public interface IIdentityContract
    {
        Task<IdentityUser> GetUserByName(string userName);
    }
}
