using mooks.authenticationservice.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mooks.authenticationservice.Domain.Storage
{
    public class StorageObject
    {
        public IList<User> Users { get; set; }
    }
}
