using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartEduSolutions.Databases.Dto;
using SmartEduSolutions.DataControl.Interfaces;
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
    public class AssignmentController : Controller
    {
        private readonly IAssignment _service;
        private readonly ILogger<AssignmentController> _logger;
        public AssignmentController(IAssignment service, ILogger<AssignmentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region List of Assignment

        // GET: api/v1.0/Assignment/list/5
        [HttpGet]
        [Route("list/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //here id is Classroom Id
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                var data = await _service.GetAssignmentList(userId, id);
                if (data != null && data.Count() != 0)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Assignment Controller, Get not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Find Assignment

        // GET: api/v1.0/Assignment/find/5
        [HttpGet]
        [Route("find/{id}")]
        public async Task<IActionResult> Find(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                var data = await _service.FindAssignment(userId, id);
                if (data != null)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Assignment Controller, Find not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Update data

        // PUT: api/v1.0/Assignment/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put([FromBody] AssignmentDto assignmentDto, int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (userId != assignmentDto.IdUsers)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    if (id != assignmentDto.IdAssignments)
                    {
                        return BadRequest();
                    }
                    var Id = await _service.UpdateAssignment(id, assignmentDto);
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
                _logger.LogError("Assignment Controller, Put not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Create new

        //POST: api/v1.0/Assignment/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AssignmentDto>> Post([FromBody] AssignmentDto assignmentDto)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if(userId != assignmentDto.IdUsers)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var Id = await _service.AddAssignment(assignmentDto);
                    if (Id > 0)
                    {
                        assignmentDto.IdAssignments = Id;
                        return Created($"api/v1.0/Assignment/{assignmentDto.IdAssignments}", assignmentDto);
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
                _logger.LogError("Assignment Controller, Post not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Permanently delete

        // DELETE: api/v1.0/Assignment/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.DeleteAssignment(userId, id);
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
                _logger.LogError("Assignment Controller, Delete not found. " + ex);
                return BadRequest();
            }
        }

        #endregion
    }
}
