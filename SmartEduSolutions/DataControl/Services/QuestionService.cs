using Microsoft.Extensions.Logging;
using SmartEduSolutions.Databases;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Databases.SEDB;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Services
{
    public class QuestionService : IQuestion
    {
        private readonly SEDBContext _context;
        private readonly ILogger<QuestionService> _logger;
        private readonly IClassroom _classroomService;

        public QuestionService(SEDBContext context, ILogger<QuestionService> logger, IClassroom classroomService)
        {
            _context = context;
            _logger = logger;
            _classroomService = classroomService;
        }

        #region Active list
        public async Task<IEnumerable<QuestionDto>> GetQuestionList(int userId, int classroomId, int assignmentId)
        {
            try
            {
                var userCheck = _classroomService.FindClassroom(userId, classroomId);
                if ((userCheck.Result != null) &&
                    (userCheck.Result.Role == Roles.Creator ||
                    userCheck.Result.Role == Roles.Teacher ||
                    userCheck.Result.Role == Roles.Student))
                {
                    var data = _context.Questions
                                    .Where(x => x.Assignments_IdAssignments.Equals(assignmentId))
                                    .OrderBy(x => x.IdQuestions)
                                    .Select(x => new QuestionDto
                                    {
                                        IdQuestions = x.IdQuestions,
                                        QuestionPart = x.QuestionPart,
                                        Marks = x.Marks,
                                        IdClassrooms = x.Classrooms_IdClassrooms,
                                        IdAssignments = x.Assignments_IdAssignments,
                                    });

                    return await Task.FromResult(data);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Question Repository, Get Question List not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Find by Id
        public async Task<QuestionDto> FindQuestion(int userId, int id)
        {
            try
            {
                var data = _context.Questions
                                    .Where(x => x.IdQuestions.Equals(id))
                                    .Select(x => new QuestionDto
                                    {
                                        IdQuestions = x.IdQuestions,
                                        QuestionPart = x.QuestionPart,
                                        Marks = x.Marks,
                                        IdClassrooms = x.Classrooms_IdClassrooms,
                                        IdAssignments = x.Assignments_IdAssignments,
                                    })
                                    .FirstOrDefault();

                var userCheck = _classroomService.FindClassroom(userId, data.IdClassrooms);
                if (userCheck.Result.Role == Roles.Creator ||
                    userCheck.Result.Role == Roles.Teacher ||
                    userCheck.Result.Role == Roles.Student)
                {
                    return await Task.FromResult(data);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Question Repository, Find Question not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Update data
        public async Task<int> UpdateQuestion(int userId, int id, QuestionDto questionDto)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();
                    var question = await _context.Questions.FindAsync(id);
                    if (question == null)
                    {
                        return 0;
                    }
                    var userCheck = _classroomService.FindClassroom(userId, questionDto.IdClassrooms);
                    if (userCheck.Result.Role == Roles.Creator ||
                        userCheck.Result.Role == Roles.Teacher)
                    {
                        question.QuestionPart = questionDto.QuestionPart;
                        question.Marks = questionDto.Marks;
                        question.UpdatedAt = currentTime;

                        await _context.SaveChangesAsync();
                        return question.IdQuestions;
                    }

                    return 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Question Repository, Update Question not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Create New
        public async Task<int> AddQuestion(int userId, QuestionDto questionDto)
        {
            if (_context != null)
            {
                try
                {
                    var userCheck = _classroomService.FindClassroom(userId, questionDto.IdClassrooms);
                    if (userCheck.Result.Role == Roles.Creator ||
                        userCheck.Result.Role == Roles.Teacher)
                    {
                        var currentTime = DateConverter.GetCurrentLocalTime();

                        Questions questions = new Questions
                        {
                            QuestionPart = questionDto.QuestionPart,
                            Marks = questionDto.Marks,
                            CreatedAt = currentTime,
                            UpdatedAt = currentTime,
                            Classrooms_IdClassrooms = questionDto.IdClassrooms,
                            Assignments_IdAssignments = questionDto.IdAssignments,
                        };
                        await _context.Questions.AddAsync(questions);
                        await _context.SaveChangesAsync();

                        return questions.IdQuestions;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Question Repository, Add Question not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Permanently delete
        public async Task<int> DeleteQuestion(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var question = await _context.Questions.FindAsync(id);
                    if (question == null)
                    {
                        return 0;
                    }

                    var userCheck = _classroomService.FindClassroom(userId, question.Classrooms_IdClassrooms);
                    if (userCheck.Result.Role == Roles.Creator ||
                        userCheck.Result.Role == Roles.Teacher)
                    {
                        _context.Questions.Remove(question);
                        await _context.SaveChangesAsync();
                        return question.IdQuestions;
                    }

                    return 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Question Repository, Delete Question not found. " + ex);
                }
            }
            return 0;
        }

        #endregion
    }
}
