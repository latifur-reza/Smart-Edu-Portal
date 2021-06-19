using SmartEduSolutions.Databases.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Interfaces
{
    public interface IAssignment
    {
        Task<IEnumerable<AssignmentDto>> GetAssignmentList(int userId, int classroomId);
        Task<AssignmentDto> FindAssignment(int userId, int id);
        Task<int> UpdateAssignment(int id, AssignmentDto assignmentDto);
        Task<int> AddAssignment(AssignmentDto assignmentDto);
        Task<int> DeleteAssignment(int userId, int id);
    }
}
