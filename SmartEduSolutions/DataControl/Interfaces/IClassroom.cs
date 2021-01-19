using SmartEduSolutions.Databases.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Interfaces
{
    public interface IClassroom
    {
        Task<IEnumerable<ClassroomForUserDto>> GetClassroomList(int userId);
        Task<ClassroomForUserDto> FindClassroom(int userId, int id);
        Task<int> UpdateClassroom(int userId, int id, ClassroomDto classroomDto);
        Task<int> AddClassroom(int userId, ClassroomDto classroomDto);
        Task<int> JoinClassroom(int userId, int id);
        Task<int> LeaveClassroom(int userId, int id);
        Task<int> DeleteClassroom(int userId, int id);
    }
}
