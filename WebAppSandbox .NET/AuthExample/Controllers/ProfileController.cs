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
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IJwtUtility _jwtUtility;

        public ProfileController(IJwtUtility jwtUtility, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _jwtUtility = jwtUtility;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Profile/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileModel>> GetProfile(string username)
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

            var resultList = await _context.Profiles.Where(profile => user.Id == profile.UserId).ToListAsync();

            return resultList.FirstOrDefault(); // TODO: Handle null?
        }

        // PATCH: api/Profile/updateAboutMe/{username}
        [HttpPatch("updateAboutMe/{username}")]
        public async Task<IActionResult> UpdateAboutMe(string username, [FromBody] string aboutMeText)
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

        // PATCH: api/Profile/updateBirthday/{username}
        /// <summary>
        /// For now this is just any string...
        /// TODO: Determine what these roles will be... Should maybe only be Teacher/Coach, Student/Trainee, Admin?
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPatch("updateRole/{username}")]
        public async Task<IActionResult> UpdateRole(string username, [FromBody] string role)
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

            var profile = await _context.Profiles.Where(profile => user.Id == profile.UserId).FirstOrDefaultAsync();

            profile.Role = role;

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