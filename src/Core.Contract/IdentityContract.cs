using Core.IRepository;
using Core.IContract;
using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.Service
{
    public class IdentityContract : IIdentityContract
    {
        private readonly IUserRepository _userRepository;

        public IdentityContract(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByName(string userName)
        {
            return await _userRepository.GetUserByName(userName);
        }
    }
}
