using Core.Models.Identity.Entities;
using EasyCaching.Core.Interceptor;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRepository
    {
        [EasyCachingPut(CacheKeyPrefix = "AspectCore")]
        Task<User> GetUserByName(string userName);
    }
}
