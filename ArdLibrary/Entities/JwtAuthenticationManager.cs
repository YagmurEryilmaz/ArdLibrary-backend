using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;

namespace ArdLibrary.Entities
{
	public class JwtAuthenticationManager
	{
		private readonly string key;

		public JwtAuthenticationManager(string key)
		{
			this.key = key;
		}

		public string Authenticate(User user)
        {
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(key);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Name, user.Name),
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

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

