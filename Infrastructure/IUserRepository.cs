using MyDMS.Domain;

namespace MyDMS.Infrastructure
{
    public interface IUserRepository
    {
        public void RegisterUserAsync(ApplicationUser user);
        public void LoginUserAsync(ApplicationUser user);
        public void LogoutUserAsync(ApplicationUser user);
    }
}
