using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArdLibrary.Data;
using ArdLibrary.Dto;
using ArdLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController:BaseController
	{
        readonly DataContext context;
        readonly IConfiguration configuration;
        string key = "ardLibraryghhfghfhfghr";
     
        public LoginController(DataContext context, IConfiguration configuration)
		{
            this.configuration = configuration;
            this.context = context;
		}
         
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<LoginResponseDto> Login(LoginDto loginDto)
        {
            User user = context.Users.FirstOrDefault(x => x.Email == loginDto.Email && x.Password ==loginDto.Password);
            if (user != null)
            {
                LoginResponseDto loginResponseDto = new LoginResponseDto();
                //create token
                JwtAuthenticationManager jwtAuthenticationManager = new JwtAuthenticationManager(key);
                loginResponseDto.AccessToken = jwtAuthenticationManager.Authenticate(user);
        

                return loginResponseDto;
            }
            return null;

        }

    }
}

