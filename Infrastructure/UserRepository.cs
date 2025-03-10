using MyDMS.Domain;

namespace MyDMS.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        void IUserRepository.LoginUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.LogoutUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.RegisterUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
