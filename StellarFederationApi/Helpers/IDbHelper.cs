using StellarFederationApi.Models;
using System.Collections.Generic;

namespace StellarFederationApi.Helpers
{
    public interface IDbHelper
    {
        void CreateAccount(Account account);
        void DeleteAccount(string federation);
        List<Account> GetAccounts();
        List<User> GetUsers();
        void UpdateAccount(Account account);
    }
}