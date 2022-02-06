namespace StellarFederationApi.Models
{
	public class UserToken
	{
		public TokenType TokenType { get; set; }
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}