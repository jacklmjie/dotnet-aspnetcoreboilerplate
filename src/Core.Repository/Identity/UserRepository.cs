using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class UserRepository
    {
        private readonly IRepository<User, long> _repository;
        public UserRepository(IRepository<User, long> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetUserByName(string userName)
        {
            return await _repository.QuerySingleOrDefaultAsync(
                "select Id,UserName,Password,NickName from User with(nolock)",
                new
                {
                    userName
                });
        }
    }
}
