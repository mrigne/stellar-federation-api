using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StellarFederationApi.Helpers;
using StellarFederationApi.Models;
using StellarFederationApi.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace StellarFederationApi.Repository
{
	public class JWTManagerRepository : IJWTManagerRepository
	{
		private readonly IConfiguration _configuration;
		private readonly IDbHelper _dbHelper;
		public JWTManagerRepository(IConfiguration configuration, IDbHelper dbHelper)
		{
			this._configuration = configuration;
			this._dbHelper = dbHelper;
		}
		public UserToken Authenticate(User user)
		{
			var users = this._dbHelper.GetUsers();
			if (!users.Any(x => x.Name == user.Name && x.PasswordHash.ToUpper() == user.PasswordHash.ToUpper()))
			{
				return null;
			}

			// Else we generate JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			 new Claim(ClaimTypes.Name, user.Name)
			  }),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new UserToken { Token = tokenHandler.WriteToken(token), TokenType = TokenType.Bearer };
		}
	}
}