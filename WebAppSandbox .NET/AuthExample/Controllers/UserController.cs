using AuthExample.Auth;
using AuthExample.Interfaces;
using AuthExample.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        private readonly IJwtUtility _jwtUtility;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IConfiguration configuration, IJwtUtility jwtUtility, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _jwtUtility= jwtUtility;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest data) // TODO: Should be async?
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

            // Change this to your google client ID
            settings.Audience = new List<string>() { _configuration.GetValue<string>("ClientId") };

            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;
            return Ok(new { AuthToken = _jwtUtility.CreateUserAuthToken(payload.Email), payload.Email });
        }

        [Authorize] // TODO: Maybe can set a debug configuration to allow this to work without authorization
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest data)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(data.Email, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var userExists = await _userManager.FindByNameAsync(data.Email);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            IdentityUser user = new()
            {
                Email = data.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = data.Email
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            var userCreated = await _userManager.FindByNameAsync(user.UserName);

            if (userCreated == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to get user. Please check user details and try again." });
            }

            var userProfile = new ProfileModel();
            userProfile.Username = user.UserName;
            userProfile.Role = "UNKNOWN"; // TODO: Set up an endpoint for clients to configure this in a setup?  Or determine how to handle profile setup...
            userProfile.Email = user.Email;
            userProfile.AboutMe = "Tell Us About Yourself...";
            userProfile.UserId = userCreated.Id;

            _context.Profiles.Add(userProfile);

            var dbResult = await _context.SaveChangesAsync();

            if (dbResult == 0) // TODO: Determine if this is the right way to error check...
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [Authorize]
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest data)
        {
            // Validate the Source and JWT token; TODO: Consider making this middleware?
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!_jwtUtility.ValidateJwtSources(data.Email, authorization))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Could not validate JWT and request source!" });
            }

            var user = await _userManager.FindByNameAsync(data.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exists!" });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User deletion failed! Please check user details and try again." });
            }

            return Ok(new Response { Status = "Success", Message = "User deleted successfully." });
        }
    }
}