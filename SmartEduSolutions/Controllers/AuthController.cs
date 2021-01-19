using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Configuration;
using SmartEduSolutions.Databases.SEDB;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartEduSolutions.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuth _service;
        private readonly ILogger<AuthController> _logger;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ClaimConfiguration _claimConfiguration;

        public AuthController(IAuth service, ILogger<AuthController> logger, IPasswordHasher<Users> passwordHasher, IOptions<ClaimConfiguration> claimConfiguration)
        {
            _service = service;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _claimConfiguration = claimConfiguration.Value;
        }

        #region Login to get token

        //POST: api/v1.0/Auth/login
        [HttpPost]
        [Route("login")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(UserDto model)
        {
            try
            {
                var user = await _service.FindUserByEmail(model.Email);
                if (user != null)
                {
                    var validation = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                    if (validation != PasswordVerificationResult.Failed)
                    {
                        var claims = GenerateClaims(user.IdUsers, user.Username, user.Email);
                        return await Token(claims);
                    }
                }
                return StatusCode(StatusCodes.Status419AuthenticationTimeout, "Email or Password not valid");
            }
            catch (Exception ex)
            {
                _logger.LogError("Auth Controller, Login not found. " + ex);
                return BadRequest();
            }
        }

        private Claim[] GenerateClaims(long userId, string username, string email)
        {
            return new Claim[]
            {
                new Claim(ClaimNames.UserId, userId.ToString()),
                new Claim(ClaimNames.Username, username),
                new Claim(ClaimNames.Email, email),
            };
        }

        private async Task<IActionResult> Token(Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_claimConfiguration.AppKey);
            var credential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var expireAt = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_claimConfiguration.ExpiredMinute));
            var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expireAt, signingCredentials: credential);
            var token = tokenHandler.WriteToken(tokenDescriptor);

            return Ok(new { access_token = token });
        }

        #endregion

        #region Registration

        //POST: api/v1.0/Auth/register
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Id = await _service.Register(userDto);
                    if (Id > 0)
                    {
                        userDto.IdUsers = Id;
                        return Created($"api/v1.0/Auth/{userDto.IdUsers}", userDto);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("Auth Controller, Register not found. " + ex);
                return BadRequest();
            }
        }

        #endregion
    }
}
