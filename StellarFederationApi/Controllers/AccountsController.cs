using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StellarFederationApi.Helpers;
using StellarFederationApi.Models;
using StellarFederationApi.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace StellarFederationApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("accounts")]
	public class AccountsController : ControllerBase
	{
		private readonly IDbHelper _dbHelper;
		public AccountsController(IDbHelper dbHelper)
		{
			this._dbHelper = dbHelper;
		}

		[AllowAnonymous]
		[HttpGet]
		public List<Account> GetAccounts()
		{
			List<Account> accountsList = new List<Account>();
			try
            {
				var accountsFromDb = this._dbHelper.GetAccounts();
				accountsList = accountsFromDb;
			} catch {}
			return accountsList;
		}

		[HttpPost]
		public Account CreateAccount(Account account)
		{
			this._dbHelper.CreateAccount(account);
			return account;
		}

		[HttpPut]
		public Account UpdateAccount(Account account)
		{
			this._dbHelper.CreateAccount(account);
			return account;
		}

		[HttpDelete("{federation}")]
		public IActionResult DeleteAccount(string federation)
		{
			this._dbHelper.DeleteAccount(federation);
			return Ok();
		}
	}
}
