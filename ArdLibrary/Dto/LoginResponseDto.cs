using System;
namespace ArdLibrary.Dto
{
	public class LoginResponseDto
	{
        public string AccessToken { get; set; }
        public UserDto UserDto { get; set; }
    }
}

