using AuthExample.Auth;
using AuthExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthExample.Controllers
{
    //[Authorize] //TODO: Determine how to protect users from manipulating other users?! Not sure if authorization token alone is enough; need to test
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Profile/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileModel>> GetProfile(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exist! Please check user details and try again." });
            }

            var resultList = await _context.Profiles.Where(profile => user.Id == profile.UserId).ToListAsync();

            return resultList.FirstOrDefault(); // TODO: Handle null?
        }

        // PATCH: api/Profile/updateAboutMe/{username}
        [HttpPatch("updateAboutMe/{username}")]
        public async Task<IActionResult> UpdateAboutMe(string username, [FromBody] string aboutMeText)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exist! Please check user details and try again." });
            }

            var profile = await _context.Profiles.Where(profile => user.Id == profile.UserId).FirstOrDefaultAsync();

            profile.AboutMe = aboutMeText;

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        // PATCH: api/Profile/updateBirthday/{username}
        [HttpPatch("updateBirthday/{username}")]
        public async Task<IActionResult> UpdateBirthday(string username, [FromBody] DateTime birthday)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exist! Please check user details and try again." });
            }

            var profile = await _context.Profiles.Where(profile => user.Id == profile.UserId).FirstOrDefaultAsync();

            profile.Birthday = birthday;

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }
    }
}