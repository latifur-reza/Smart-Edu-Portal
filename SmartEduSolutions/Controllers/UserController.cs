using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUser service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region List of users by classroom id

        // GET: api/v1.0/User/classroom/5
        [HttpGet]
        [Route("classroom/{id}")]
        public async Task<IActionResult> GetUsers(int id)
        {
            int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

            try
            {
                var data = await _service.GetUserList(userId, id);
                if (data.Count() != 0)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("User Controller, Get Users not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Make Teacher

        // PUT: api/v1.0/User/maketeacher/5
        [HttpPut("maketeacher/{id}")]
        public async Task<IActionResult> MakeTeacher(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                    var Id = await _service.MakeTeacher(userId, id);
                    if (Id > 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("User Controller, Make Teacher not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Make Student

        // PUT: api/v1.0/User/makestudent/5
        [HttpPut("makestudent/{id}")]
        public async Task<IActionResult> MakeStudent(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                    var Id = await _service.MakeStudent(userId, id);
                    if (Id > 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("User Controller, Make Student not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Permanently delete from classroom

        // DELETE: api/v1.0/User/deletefromclassroom/5
        [HttpDelete("deletefromclassroom/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.DeleteUserFromClassroom(userId, id);
                    if (Id > 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("User Controller, Delete From Classroom not found. " + ex);
                return BadRequest();
            }
        }

        #endregion
    }
}
