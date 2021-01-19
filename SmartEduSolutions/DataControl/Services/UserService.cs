using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Databases;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.DataControl.Services
{
    public class UserService : IUser
    {
        private readonly SEDBContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(SEDBContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region User list by classroom
        public async Task<IEnumerable<UserForClassroomDto>> GetUserList(int userId, int classroomId)
        {
            try
            {
                var hasClass = FindClassroom(userId, classroomId);
                if (hasClass.Result == null)
                {
                    return null;
                }

                var data = _context.Users
                                    .GroupJoin(_context.User_has_Classroom,
                                        user => user.IdUsers,
                                        user_has_classroom => user_has_classroom.Users_IdUsers,
                                        (user, user_has_classroom) => new
                                        {
                                            user,
                                            user_has_classroom
                                        })
                                    .SelectMany(temp => temp.user_has_classroom.DefaultIfEmpty(),
                                    (firstJoinUser, firstJoinUserClass) => new UserForClassroomDto
                                    {
                                        IdUserHasClassroom = firstJoinUserClass.IdUserHasClassroom,
                                        Role = firstJoinUserClass.Role,
                                        IdUsers = firstJoinUser.user.IdUsers,
                                        Username = firstJoinUser.user.Username,
                                        Email = firstJoinUser.user.Email,
                                        ProfilePic = firstJoinUser.user.ProfilePic,
                                        IdClassrooms = firstJoinUserClass.Classrooms_IdClassrooms,
                                    })
                                    .Where(temp => temp.IdClassrooms.Equals(classroomId));

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("User Repository, Get User List not found. " + ex);
                throw ex;
            }
        }

        #endregion

        #region Make Teacher
        public async Task<int> MakeTeacher(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    var user_has_classroom = await _context.User_has_Classroom.FindAsync(id);
                    if (user_has_classroom == null || user_has_classroom.Role != Roles.Student || user_has_classroom.Users_IdUsers == userId)
                    {
                        return 0;
                    }

                    user_has_classroom.Role = Roles.Teacher;
                    user_has_classroom.CreatedAt = currentTime;

                    await _context.SaveChangesAsync();
                    return user_has_classroom.Classrooms_IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("User Repository, Make Teacher not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Make Student
        public async Task<int> MakeStudent(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var currentTime = DateConverter.GetCurrentLocalTime();

                    var user_has_classroom = await _context.User_has_Classroom.FindAsync(id);
                    if (user_has_classroom == null || user_has_classroom.Role != Roles.Teacher || user_has_classroom.Users_IdUsers == userId)
                    {
                        return 0;
                    }

                    user_has_classroom.Role = Roles.Student;
                    user_has_classroom.CreatedAt = currentTime;

                    await _context.SaveChangesAsync();
                    return user_has_classroom.Classrooms_IdClassrooms;
                }
                catch (Exception ex)
                {
                    _logger.LogError("User Repository, Make Student not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Permanently delete from classroom
        public async Task<int> DeleteUserFromClassroom(int userId, int id)
        {
            if (_context != null)
            {
                try
                {
                    var user_has_classroom = await _context.User_has_Classroom.FindAsync(id);

                    if (user_has_classroom == null)
                    {
                        return 0;
                    }

                    var hasClass = FindClassroom(userId, user_has_classroom.Classrooms_IdClassrooms);
                    if (hasClass.Result == null || hasClass.Result.Role == Roles.Student)
                    {
                        return 0;
                    }

                    _context.User_has_Classroom.Remove(user_has_classroom);
                    await _context.SaveChangesAsync();
                    return user_has_classroom.IdUserHasClassroom;
                }
                catch (Exception ex)
                {
                    _logger.LogError("User Repository, Delete User From Classroom not found. " + ex);
                }
            }
            return 0;
        }

        #endregion

        #region Find Classroom by Id
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
                                    .Where(temp => temp.IdUsers.Equals(userId))
                                    .FirstOrDefault();

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("User Repository, Find Classroom not found. " + ex);
                throw ex;
            }
        }

        #endregion
    }
}
