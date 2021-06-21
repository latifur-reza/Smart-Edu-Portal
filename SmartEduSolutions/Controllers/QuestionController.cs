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
    public class QuestionController : Controller
    {
        private readonly IQuestion _service;
        private readonly ILogger<QuestionController> _logger;
        public QuestionController(IQuestion service, ILogger<QuestionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region List of Question

        // GET: api/v1.0/Question/list/6/5
        [HttpGet]
        [Route("list/{classroomId}/{assignmentId}")]
        public async Task<IActionResult> Get(int classroomId, int assignmentId)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                var data = await _service.GetQuestionList(userId, classroomId, assignmentId);
                if (data != null && data.Count() != 0)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Question Controller, Get not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Find Question

        // GET: api/v1.0/Question/find/5
        [HttpGet]
        [Route("find/{id}")]
        public async Task<IActionResult> Find(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                var data = await _service.FindQuestion(userId, id);
                if (data != null)
                {
                    return Ok(data);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Question Controller, Find not found. " + ex);
                return NoContent();
            }
        }

        #endregion

        #region Update data

        // PUT: api/v1.0/Question/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put([FromBody] QuestionDto questionDto, int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    if (id != questionDto.IdQuestions)
                    {
                        return BadRequest();
                    }
                    var Id = await _service.UpdateQuestion(userId, id, questionDto);
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
                _logger.LogError("Question Controller, Put not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Create new

        //POST: api/v1.0/Question/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<QuestionDto>> Post([FromBody] QuestionDto questionDto)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.AddQuestion(userId, questionDto);
                    if (Id > 0)
                    {
                        questionDto.IdQuestions = Id;
                        return Created($"api/v1.0/Question/{questionDto.IdQuestions}", questionDto);
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
                _logger.LogError("Question Controller, Post not found. " + ex);
                return BadRequest();
            }
        }

        #endregion

        #region Permanently delete

        // DELETE: api/v1.0/Question/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimNames.UserId).Value.ToString());

                if (ModelState.IsValid)
                {
                    var Id = await _service.DeleteQuestion(userId, id);
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
                _logger.LogError("Question Controller, Delete not found. " + ex);
                return BadRequest();
            }
        }

        #endregion
    }
}
