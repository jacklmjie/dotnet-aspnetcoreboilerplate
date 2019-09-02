using Core.IRepository;
using Core.Models.Identity.Entities;
using Core.Repository.Infrastructure;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly IRepository<User, long> _repository;
        public UserRepository(IRepository<User, long> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetUserByName(string userName)
        {
            return await _repository.QuerySingleOrDefaultAsync(
                "select Id,UserName,Password,NickName from User",
                new
                {
                    userName
                });
        }
    }
}
