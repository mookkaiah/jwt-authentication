using mooks.authenticationservice.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mooks.authenticationservice.Services
{
    public interface IStorageService
    {
        public Task<IList<User>> GetAllUsers();

        public Task<bool> AddUser(User userNew);

        public Task<bool> UpdateUser(User user);

        public Task<bool> ChangePassword(User user);
    }
}
