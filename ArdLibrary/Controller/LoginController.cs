using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArdLibrary.Data;
using ArdLibrary.Dto;
using ArdLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController:ControllerBase
	{
        readonly DataContext context;
        readonly IConfiguration configuration;
        string key = "ardLibrary";
        byte[] passwordHash, passwordSalt;

        public LoginController(DataContext context, IConfiguration configuration)
		{
            this.configuration = configuration;
            this.context = context;
		}

        [HttpPost("action")]
        public async Task<LoginResponseDto> Login([FromBody] LoginDto loginDto)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == loginDto.Id && x.Password ==loginDto.Password);
            if (user != null)
            {
                LoginResponseDto loginResponseDto = new LoginResponseDto();
                //create token
                JwtAuthenticationManager jwtAuthenticationManager = new JwtAuthenticationManager(key);
                loginResponseDto.AccessToken = jwtAuthenticationManager.Authenticate(user.Id.ToString());
                loginResponseDto.UserDto.Id = loginDto.Id;
   
                return loginResponseDto;
            }
            return null;

        }

    }
}

