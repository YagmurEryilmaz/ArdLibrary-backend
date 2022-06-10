using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ArdLibrary.Entities
{
	public class JwtAuthenticationManager
	{
		private readonly string key;
		private readonly IDictionary<string, string> users = new Dictionary<string, string>()
		{
			{"1","password"}
		};

		public JwtAuthenticationManager(string key)
		{
			this.key = key;
		}

		public string Authenticate(string userId, string password)
        {
			if(!users.Any(u=>u.Key==userId && u.Value==password))
			{ return null; }
			else
            {
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.ASCII.GetBytes(key);

				SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.NameIdentifier, userId)
					}),

					Expires = DateTime.UtcNow.AddHours(1),
					SigningCredentials = new SigningCredentials(
						new SymmetricSecurityKey(tokenKey),
						SecurityAlgorithms.HmacSha256Signature)

				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				return tokenHandler.WriteToken(token);

            }
        }
	}
}

