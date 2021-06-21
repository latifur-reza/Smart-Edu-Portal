using SmartEduSolutions.Databases.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Interfaces
{
    public interface IQuestion
    {
        Task<IEnumerable<QuestionDto>> GetQuestionList(int userId, int classroomId, int assignmentId);
        Task<QuestionDto> FindQuestion(int userId, int id);
        Task<int> UpdateQuestion(int userId, int id, QuestionDto questionDto);
        Task<int> AddQuestion(int userId, QuestionDto questionDto);
        Task<int> DeleteQuestion(int userId, int id);
    }
}
