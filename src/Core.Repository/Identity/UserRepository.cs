using Core.IRepository;
using Core.Models.Identity.Entities;
using Core.Repository.Infrastructure;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly IRepository<IdentityUser, long> _repository;
        public UserRepository(IRepository<IdentityUser, long> repository)
        {
            _repository = repository;
        }

        public async Task<IdentityUser> GetUserByName(string userName)
        {
            return await _repository.QuerySingleOrDefaultAsync(
                "select Id,UserName,Password,NickName from IdentityUser WHERE UserName=@UserName",
                new
                {
                    userName
                });
        }
    }
}
