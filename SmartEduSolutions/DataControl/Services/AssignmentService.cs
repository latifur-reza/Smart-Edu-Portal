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
    public class AssignmentService : IAssignment
    {
        private readonly SEDBContext _context;
        private readonly ILogger<AssignmentService> _logger;
        private readonly IClassroom _classroomService;

        public AssignmentService(SEDBContext context, ILogger<AssignmentService> logger, IClassroom classroomService)
        {
            _context = context;
            _logger = logger;
            _classroomService = classroomService;
        }

        #region Active list
        public async Task<IEnumerable<AssignmentDto>> GetAssignmentList(int userId, int classroomId)
        {
            try
            {
                var userCheck = _classroomService.FindClassroom(userId, classroomId);
                if ((userCheck.Result != null) &&
                    (userCheck.Result.Role == Roles.Creator ||
                    userCheck.Result.Role == Roles.Teacher ||
                    userCheck.Result.Role == Roles.Student))
                {
                    var data = _context.Assignments
                                    .Where(x => x.Classrooms_IdClassrooms.Equals(classroomId))
                                    .OrderBy(x => x.StartedAt)
                                    .Select(x => new AssignmentDto
                                    {
                                        IdAssignments = x.IdAssignments,
                                        Title = x.Title,
                                        TotalMarks = x.TotalMarks,
                                        StartedAt = x.StartedAt,
                                        EndedAt = x.EndedAt,
                                        IdClassrooms = x.Classrooms_IdClassrooms,
                                        IdUsers = x.Users_IdUsers,
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
                _logger.LogError("Assignment Repository, Get Assignment List not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Find by Id
        public async Task<AssignmentDto> FindAssignment(int userId, int id)
        {
            try
            {
                var data = _context.Assignments
                                    .Where(x => x.IdAssignments.Equals(id))
                                    .OrderBy(x => x.StartedAt)
                                    .Select(x => new AssignmentDto
                                    {
                                        IdAssignments = x.IdAssignments,
                                        Title = x.Title,
                                        TotalMarks = x.TotalMarks,
                                        StartedAt = x.StartedAt,
                                        EndedAt = x.EndedAt,
                                        IdClassrooms = x.Classrooms_IdClassrooms,
                                        IdUsers = x.Users_IdUsers,
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
                _logger.LogError("Assignment Repository, Find Assignment not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Update data
        public async Task<int> UpdateAssignment(int id, AssignmentDto assignmentDto)
        {
            if (_context != null)
            {
                try
                {
                    var assignment = await _context.Assignments.FindAsync(id);
                    if (assignment == null)
                    {
                        return 0;
                    }

                    if(assignment.Users_IdUsers != assignmentDto.IdUsers)
                    {
                        return 0;
                    }

                    assignment.Title = assignmentDto.Title;
                    assignment.TotalMarks = assignmentDto.TotalMarks;
                    assignment.StartedAt = assignmentDto.StartedAt;
                    assignment.EndedAt = assignmentDto.EndedAt;

                    await _context.SaveChangesAsync();
                    return assignment.IdAssignments;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Assignment Repository, Update Assignment not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Create New
        public async Task<int> AddAssignment(AssignmentDto assignmentDto)
        {
            if (_context != null)
            {
                try
                {
                    var userCheck = _classroomService.FindClassroom(assignmentDto.IdUsers, assignmentDto.IdClassrooms);
                    if (userCheck.Result.Role == Roles.Creator ||
                        userCheck.Result.Role == Roles.Teacher)
                    {
                        var currentTime = DateConverter.GetCurrentLocalTime();

                        Assignments assignments = new Assignments
                        {
                            Title = assignmentDto.Title,
                            TotalMarks = assignmentDto.TotalMarks,
                            StartedAt = assignmentDto.StartedAt,
                            EndedAt = assignmentDto.EndedAt,
                            CreatedAt = currentTime,
                            Classrooms_IdClassrooms = assignmentDto.IdClassrooms,
                            Users_IdUsers = assignmentDto.IdUsers,
                        };
                        await _context.Assignments.AddAsync(assignments);
                        await _context.SaveChangesAsync();

                        return assignments.IdAssignments;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Assignment Repository, Add Assignment not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Permanently delete
        public async Task<int> DeleteAssignment(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var assignment = await _context.Assignments.FindAsync(id);
                    if (assignment == null)
                    {
                        return 0;
                    }

                    if(assignment.Users_IdUsers != userId)
                    {
                        return 0;
                    }

                    _context.Assignments.Remove(assignment);
                    await _context.SaveChangesAsync();
                    return assignment.IdAssignments;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Assignment Repository, Delete Assignment not found. " + ex);
                }
            }
            return 0;
        }

        #endregion
    }
}
