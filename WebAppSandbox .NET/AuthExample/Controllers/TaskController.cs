using AuthExample.Auth;
using AuthExample.Interfaces;
using AuthExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace AuthExample.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IJwtUtility _jwtUtility;

        public TaskController(IJwtUtility jwtUtility, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _jwtUtility = jwtUtility;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Task
        [HttpGet("getTasks")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTaskItems(string username)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(username, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var user = await _userManager.FindByNameAsync(username); 

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exist! Please check user details and try again." });
            }

            var resultList = await _context.Tasks.Where(task => user.Id == task.UserId).ToListAsync();

            return resultList;
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTaskModel(string username, int id)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(username, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var taskModel = await _context.Tasks.FindAsync(id);

            if (taskModel == null)
            {
                return NotFound();
            }

            return taskModel;
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskModel(string username, int id, TaskModel task)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(username, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            if (id != task.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskModel>> PostTaskModel(string username, string taskName, string taskDescription)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(username, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var user = await _userManager.FindByNameAsync(username); 

            if (user != null) {
                var taskToAdd = new TaskModel();
                taskToAdd.TaskName = taskName;
                taskToAdd.TaskDescription = taskDescription;
                taskToAdd.TaskStatus = false;
                taskToAdd.UserId = user.Id;

                _context.Tasks.Add(taskToAdd);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTaskModel", new { id = taskToAdd.TaskId }, taskToAdd); // TODO: Why does this return a 201?
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Task creation failed! Please check user details and try again." });
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModel(string username, int id)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(username, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var taskModel = await _context.Tasks.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskModelExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}