using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyDMS.Application;
using MyDMS.Domain;
using System.Threading.Tasks;

namespace MyDMS.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(TokenService tokenService,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var user = new ApplicationUser {
                UserName = registerRequest.Username
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(registerRequest.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerRequest.Role));
            }

            await _userManager.AddToRoleAsync(user, registerRequest.Role);

            return Ok();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            
            if(!result.Succeeded)
            {
                return Unauthorized("Invalid password");
            }   

            var token = _tokenService.GenerateToken(request.Username, user.Id, (await _userManager.GetRolesAsync(user))[0]);
            var refreshToken = _tokenService.GenerateRefreshToken();
            _tokenService.StoreRefreshToken(user.Id, refreshToken);
            return Ok(new { Token = token, RefreshToken = refreshToken});
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync(); // jwts are stateless so this is not necessary but its here for readability and consistency i gueeeesss
            return Ok();
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
