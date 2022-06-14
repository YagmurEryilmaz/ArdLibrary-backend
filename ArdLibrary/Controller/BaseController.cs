using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace ArdLibrary.Controller
{
    [Route("api/[controller]")]
    [ApiController]
	public class BaseController: ControllerBase
	{
        protected int GetUserId()
        {
            var token = string.Empty;
            var header = (string)HttpContext.Request.Headers["Authorization"];
            if (header != null) { token = header.Substring(7); }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            int userId = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);

            return userId;

        }
    }
}

