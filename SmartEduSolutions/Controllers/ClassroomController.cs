using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClassroomController : Controller
    {
        private readonly IClassroom _service;
        private readonly ILogger<ClassroomController> _logger;
        public ClassroomController(IClassroom service, ILogger<ClassroomController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region List of classrooms

        // GET: api/v1.0/Classroom/list
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Get()
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                var data = await _service.GetClassroomList(userId);
                if (data.Count() != 0)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Classroom Controller, Get not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Update data

        // PUT: api/v1.0/Classroom/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put([FromBody] ClassroomDto classroomDto, int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    if (id != classroomDto.IdClassrooms)
                    {
                        return BadRequest();
                    }
                    var Id = await _service.UpdateClassroom(userId, id, classroomDto);
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
                _logger.LogError("Classroom Controller, Put not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Create new

        //POST: api/v1.0/Classroom/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ClassroomDto>> Post([FromBody] ClassroomDto classroomDto)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.AddClassroom(userId, classroomDto);
                    if (Id > 0)
                    {
                        classroomDto.IdClassrooms = Id;
                        return Created($"api/v1.0/Classroom/{classroomDto.IdClassrooms}", classroomDto);
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
                _logger.LogError("Classroom Controller, Post not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Join new class

        //PUT: api/v1.0/Classroom/join/5
        [HttpPut]
        [Route("join/{id}")]
        public async Task<ActionResult> Join(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.JoinClassroom(userId, id);
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
                _logger.LogError("Classroom Controller, Join not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Leave class

        //POST: api/v1.0/Classroom/leave/5
        [HttpPut]
        [Route("leave/{id}")]
        public async Task<ActionResult<ClassroomDto>> Leave(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.LeaveClassroom(userId, id);
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
                _logger.LogError("Classroom Controller, Leave not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Permanently delete

        // DELETE: api/v1.0/Classroom/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.DeleteClassroom(userId, id);
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
                _logger.LogError("Classroom Controller, Delete not found. " + ex);
                return BadRequest();
            }
        }

        #endregion
    }
}
