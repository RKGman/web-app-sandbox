using AuthExample.Auth;
using AuthExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthExample.Controllers
{
    //[Authorize] TODO: Determine how to protect users from manipulating other users?! Not sure if authorization token alone is enough; need to test
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Task
        [HttpGet("getTasks")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTaskItems(string username)
        {
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
        public async Task<ActionResult<TaskModel>> GetTaskModel(int id)
        {
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
        public async Task<IActionResult> PutTaskModel(int id, TaskModel task)
        {
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
        public async Task<IActionResult> DeleteTaskModel(int id)
        {
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