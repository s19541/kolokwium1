using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly Services.ITaskDbService _dbService;
        public TaskController(Services.ITaskDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet("{idProject}")]
        public IActionResult getTask(int idProject)
        {
            try
            {
                var taskList = _dbService.GetTasks(idProject);
                if (taskList != null)
                    return Ok(taskList);
            }catch(Exception e)
            {
                return NotFound(e.GetBaseException());
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult addTask(Requests.TaskRequest request)
        {
            try
            {
                _dbService.addTask(request);
            }catch(Exception e)
            {
                return NotFound(e.GetBaseException());
            }
            return Ok();
        }

    }
}