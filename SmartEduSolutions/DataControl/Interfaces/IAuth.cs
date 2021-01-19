using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Interfaces
{
    public interface IAuth
    {
        Task<Users> FindUserByEmail(string email);
        Task<int> Register(UserDto userDto);
    }
}
