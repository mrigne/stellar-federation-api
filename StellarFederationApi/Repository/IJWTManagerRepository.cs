using StellarFederationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StellarFederationApi.Repository
{
    public interface IJWTManagerRepository
    {
        UserToken Authenticate(User users);
    }

}