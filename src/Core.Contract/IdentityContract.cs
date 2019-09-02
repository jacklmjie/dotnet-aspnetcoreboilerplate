using Core.IRepository;
using Core.IContract;
using System.Threading.Tasks;
using Core.Models.Identity.Entities;

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
