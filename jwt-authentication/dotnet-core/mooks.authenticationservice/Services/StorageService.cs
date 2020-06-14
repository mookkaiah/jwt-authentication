using Microsoft.Extensions.Options;
using mooks.authenticationservice.Data.Options;
using mooks.authenticationservice.Domain.Identity;
using mooks.authenticationservice.Domain.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mooks.authenticationservice.Services
{
    public class StorageService : IStorageService
    {
        private readonly IOptions<StorageOptions> _storageOptions;

        private StorageObject _storageObject;
        public StorageService(IOptions<StorageOptions> storageOptions)
        {
            _storageOptions = storageOptions;
            ReadStorageObject();
        }
        public async Task<bool> AddUser(User userNew)
        {
            _storageObject.Users.Add(userNew);

            var userAdded = await WriteStorageObject(_storageObject);

            return userAdded;
        }

        public async Task<IList<User>> GetAllUsers()
        {
            return  _storageObject.Users;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var updatedUser = _storageObject.Users.Where(x => x.Email == user.Email).Select(x => { x.Id = user.Id; x.Password = user.Password; return x; }).FirstOrDefault();
            var userAdded = await WriteStorageObject(_storageObject);

            return userAdded;
        }

        public async Task<bool> ChangePassword(User user)
        {
            var updatedUser = _storageObject.Users.Where(x => x.Email == user.Email).Select(x => { x.Password = user.Password; return x; }).FirstOrDefault();
            var passwordChanged = await WriteStorageObject(_storageObject);

            return passwordChanged;
        }

        private void ReadStorageObject()
        {
            var JSON = System.IO.File.ReadAllText(_storageOptions.Value.IdentifyFileName);
            _storageObject = Newtonsoft.Json.JsonConvert.DeserializeObject<StorageObject>(JSON);
        }

        private async Task<bool> WriteStorageObject(StorageObject storageObject)
        {
            var storageObjectJson = Newtonsoft.Json.JsonConvert.SerializeObject(storageObject);
            System.IO.File.WriteAllText(_storageOptions.Value.IdentifyFileName, storageObjectJson);

            return true;
        }
    }
}