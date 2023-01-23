using AuthExample.Auth;
using AuthExample.Models;
using AuthService.Utilities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly JwtGenerator _jwtGenerator;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IConfiguration configuration, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _jwtGenerator = new JwtGenerator(configuration.GetValue<string>("JwtPrivateSigningKey"));
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest data) // TODO: Should be async?
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

            // Change this to your google client ID
            settings.Audience = new List<string>() { "<CLIENT_ID>" }; // TODO: Read this from a secrets file

            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;
            return Ok(new { AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email), payload.Email });
        }

        [Authorize] // TODO: Maybe can set a debug configuration to allow this to work without authorization
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest data)
        {
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

            //ApplicationUser appUser = new ApplicationUser()
            //{

            //};
            //_context.Users.Add()

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [Authorize]
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest data)
        {
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