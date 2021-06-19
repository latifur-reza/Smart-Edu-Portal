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
    public class ClassroomService : IClassroom
    {
        private readonly SEDBContext _context;
        private readonly ILogger<ClassroomService> _logger;

        public ClassroomService(SEDBContext context, ILogger<ClassroomService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Active list
        public async Task<IEnumerable<ClassroomForUserDto>> GetClassroomList(int userId)
        {
            try
            {
                var data = _context.Classrooms
                                    .GroupJoin(_context.User_has_Classroom,
                                        classroom => classroom.IdClassrooms,
                                        user_has_classroom => user_has_classroom.Classrooms_IdClassrooms,
                                        (classroom, user_has_classroom) => new
                                        {
                                            classroom,
                                            user_has_classroom
                                        })
                                    .SelectMany(temp => temp.user_has_classroom.DefaultIfEmpty(),
                                    (firstJoinClass, firstJoinUserClass) => new ClassroomForUserDto
                                    {
                                        IdClassrooms = firstJoinClass.classroom.IdClassrooms,
                                        Title = firstJoinClass.classroom.Title,
                                        CoverPic = firstJoinClass.classroom.CoverPic,
                                        Role = firstJoinUserClass.Role,
                                        IdUsers = firstJoinUserClass.Users_IdUsers,
                                    })
                                    .OrderByDescending(x => x.IdClassrooms)
                                    .Where(temp => temp.IdUsers.Equals(userId));

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Classroom Repository, Get Classroom List not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Find by Id
        public async Task<ClassroomForUserDto> FindClassroom(int userId, int id)
        {
            try
            {
                var data = _context.Classrooms
                                    .Where(x => x.IdClassrooms.Equals(id))
                                    .GroupJoin(_context.User_has_Classroom,
                                        classroom => classroom.IdClassrooms,
                                        user_has_classroom => user_has_classroom.Classrooms_IdClassrooms,
                                        (classroom, user_has_classroom) => new
                                        {
                                            classroom,
                                            user_has_classroom
                                        })
                                    .SelectMany(temp => temp.user_has_classroom.DefaultIfEmpty(),
                                    (firstJoinClass, firstJoinUserClass) => new ClassroomForUserDto
                                    {
                                        IdUserHasClassroom = firstJoinUserClass.IdUserHasClassroom,
                                        IdClassrooms = firstJoinClass.classroom.IdClassrooms,
                                        Title = firstJoinClass.classroom.Title,
                                        CoverPic = firstJoinClass.classroom.CoverPic,
                                        Role = firstJoinUserClass.Role,
                                        IdUsers = firstJoinUserClass.Users_IdUsers,
                                    })
                                    .OrderByDescending(x => x.IdUserHasClassroom)
                                    .Where(temp => temp.IdUsers.Equals(userId))
                                    .FirstOrDefault();

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Classroom Repository, Find Classroom not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Update data
        public async Task<int> UpdateClassroom(int userId, int id, ClassroomDto classroomDto)
        {
            if (_context != null)
            {
                try
                {
                    var classroom = await _context.Classrooms.FindAsync(id);
                    if (classroom == null)
                    {
                        return 0;
                    }
                    
                    classroom.Title = classroomDto.Title;
                    classroom.CoverPic = classroomDto.CoverPic;

                    await _context.SaveChangesAsync();
                    return classroom.IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Classroom Repository, Update Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Create New
        public async Task<int> AddClassroom(int userId, ClassroomDto classroomDto)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    Classrooms classroom = new Classrooms
                    {
                        Title = classroomDto.Title,
                        CoverPic = classroomDto.CoverPic,

                        CreatedAt = currentTime,
                    };
                    await _context.Classrooms.AddAsync(classroom);
                    await _context.SaveChangesAsync();

                    User_has_Classroom user_Has_Classroom = new User_has_Classroom
                    {
                        Role = Roles.Creator,
                        CreatedAt = currentTime,
                        Users_IdUsers = userId,
                        Classrooms_IdClassrooms = classroom.IdClassrooms,
                    };
                    await _context.User_has_Classroom.AddAsync(user_Has_Classroom);
                    await _context.SaveChangesAsync();

                    return classroom.IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Classroom Repository, Add Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Join New Class
        public async Task<int> JoinClassroom(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    var classroom = await _context.Classrooms.FindAsync(id);
                    if (classroom == null)
                    {
                        return 0;
                    }
                    var hasClass = FindClassroom(userId, id);
                    if(hasClass.Result != null)
                    {
                        return 0;
                    }

                    User_has_Classroom user_Has_Classroom = new User_has_Classroom
                    {
                        Role = Roles.Student,
                        CreatedAt = currentTime,
                        Users_IdUsers = userId,
                        Classrooms_IdClassrooms = id,
                    };
                    await _context.User_has_Classroom.AddAsync(user_Has_Classroom);
                    await _context.SaveChangesAsync();

                    return classroom.IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Classroom Repository, Join Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Leave Class
        public async Task<int> LeaveClassroom(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    var classroom = await _context.Classrooms.FindAsync(id);
                    if (classroom == null)
                    {
                        return 0;
                    }
                    var hasClass = FindClassroom(userId, id);
                    if (hasClass.Result == null || hasClass.Result.Role == Roles.Creator)
                    {
                        return 0;
                    }

                    var user_has_classroom = await _context.User_has_Classroom.FindAsync(hasClass.Result.IdUserHasClassroom);
                    if (user_has_classroom == null)
                    {
                        return 0;
                    }

                    _context.User_has_Classroom.Remove(user_has_classroom);
                    await _context.SaveChangesAsync();
                    return user_has_classroom.IdUserHasClassroom;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Classroom Repository, Leave Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Permanently delete
        public async Task<int> DeleteClassroom(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var classroom = await _context.Classrooms.FindAsync(id);
                    if (classroom == null)
                    {
                        return 0;
                    }

                    _context.Classrooms.Remove(classroom);
                    await _context.SaveChangesAsync();
                    return classroom.IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Classroom Repository, Delete Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion
    }
}
