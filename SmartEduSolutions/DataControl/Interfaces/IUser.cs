using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<UserForClassroomDto>> GetUserList(int userId, int classroomId);
        Task<int> MakeTeacher(int userId, int id);
        Task<int> MakeStudent(int userId, int id);
        Task<int> DeleteUserFromClassroom(int userId, int id);
    }
}
