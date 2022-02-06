using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace StellarFederationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IDbHelper _dbHelper;
		public UserController(IJWTManagerRepository jWTManager, IDbHelper dbHelper)
		{
			this._jWTManager = jWTManager;
			this._dbHelper = dbHelper;
		}

		[HttpPost]
		[Route("/signin")]
		public IActionResult Authenticate([Required] LoginCredentials loginCredentials)
		{
            UserToken token = _jWTManager.Authenticate(new User { Name = loginCredentials.Login, PasswordHash = MD5Helper.CreateMD5(loginCredentials.Password) });

			if (token == null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}

		[Authorize]
		[HttpGet]
		[Route("/whoami")]
		public WhoAmIResponse GetWhoAmI()
        {
			var userId = User.FindFirst(ClaimTypes.Name).Value;
			return new WhoAmIResponse { 
				Username = userId
			};
        }
	}
}
