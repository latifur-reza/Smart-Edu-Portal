using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Databases;
using SmartEduSolutions.Databases.SEDB;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Services
{
    public class AuthService : IAuth
    {
        private readonly SEDBContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<Users> _passwordHasher;

        public AuthService(SEDBContext context, ILogger<AuthService> logger, IPasswordHasher<Users> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        #region Find by Email
        public async Task<Users> FindUserByEmail(string email)
        {
            try
            {
                var data = _context.Users
                                    .Where(x => x.Email.Equals(email))
                                    .Select(x => new Users
                                    {
                                        IdUsers = x.IdUsers,
                                        Username = x.Username,
                                        Email = x.Email,
                                        Password = x.Password,
                                        ProfilePic = x.ProfilePic,
                                        CreatedAt = x.CreatedAt
                                    })
                                    .FirstOrDefault();
                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Auth Repository, Find User by Email not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Registration
        public async Task<int> Register(UserDto userDto)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    Users user = new Users
                    {
                        Username = userDto.Username,
                        Email = userDto.Email,
                        ProfilePic = userDto.ProfilePic,

                        CreatedAt = currentTime,
                    };
                    user.Password = _passwordHasher.HashPassword(user, userDto.Password);
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return user.IdUsers;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Auth Repository, Register not found. " + ex);
                }
            }
            return 0;
        }

        #endregion
    }
}
