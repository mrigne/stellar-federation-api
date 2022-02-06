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
using Microsoft.Extensions.Configuration;
using System.IO;

namespace StellarFederationApi.Controllers
{
	[ApiController]
	[Route("federation")]
	public class FederationController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IDbHelper _dbHelper;
		public FederationController(IConfiguration configuration, IDbHelper dbHelper)
		{
			this._configuration = configuration;
			this._dbHelper = dbHelper;
		}

		[HttpGet]
		public IActionResult GetFederationResponse([FromQuery] string q, [FromQuery] string type)
		{
			if (type == "name")
            {
				List<Account> accountsList = new List<Account>();
				try
				{
					var accountsFromDb = this._dbHelper.GetAccounts();
					accountsList = accountsFromDb;
				}
				catch { }
				var correspondingAccount = accountsList.Find(accountFromDb => accountFromDb.Federation == q.Replace($"*{_configuration["Federation:Host"]}", ""));
				if (correspondingAccount == null)
                {
					return StatusCode(404);
				} else
                {
					return Ok(new FederationResponse { 
						StellarAddress = q,
						AccountId = correspondingAccount.Address,
						MemoType = correspondingAccount.MemoType,
						Memo = correspondingAccount.Memo
					});
                }
			} else
            {
				return StatusCode(500, "Not supported query type");
            }
			
		}

		[HttpGet]
		[Route("/.well-known/stellar.toml")]
		public string GetStellarTomlFileContent()
		{
			return $"FEDERATION_SERVER=\"https://{_configuration["Federation:Host"]}:{_configuration["Federation:Port"]}/federation\"";
		}
	}
}
