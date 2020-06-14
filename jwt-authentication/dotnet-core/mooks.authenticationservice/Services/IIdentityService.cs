using mooks.authenticationservice.contracts.response;
using mooks.authenticationservice.Domain.Identity;
using System.Threading.Tasks;

namespace mooks.authenticationservice.Services
{
    public interface IIdentityService
    {
        Task<RegistrationResult> RegisterAsync(User newUser);

        Task<LoginResult> LoginAsync(User loginUser);

        Task<UpdateUserResult> UpdateUser(User updateUser);

        Task<UpdateUserResult> ChangePassword(User changePasswordUser);
    }
}
