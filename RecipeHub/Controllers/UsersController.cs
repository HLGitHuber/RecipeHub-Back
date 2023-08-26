using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using System.Security.Claims;

namespace RecipeHub.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController
            (UserManager<User> userManager,SignInManager<User> signInManager,
            ILogger<UsersController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpOptions("regiester")]
        [ResponseCache(CacheProfileName = "NoCache")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto userForRegistrationDto) 
        {
            var user = new User();
            user.UserName = userForRegistrationDto.UserName;
            user.Email = userForRegistrationDto.Email;

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            if(!result.Succeeded)
            {
                _logger.LogWarning($"User creation for {userForRegistrationDto.UserName} failed");
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new {Errors = errors});
            }
            _logger.LogInformation($"User {userForRegistrationDto.UserName} created");
            result = await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);

            if(!result.Succeeded)
            {
                _logger.LogWarning($"User {userForRegistrationDto.UserName} could not be assiged roles");
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            _logger.LogWarning($"User {userForRegistrationDto.UserName} assiged to roles");
            return Accepted($"User {user.UserName} created");
        }

        [HttpOptions("login-cookie")]
        [ResponseCache(CacheProfileName = "NoCache")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //make a choice 400 vs 401
        public async Task<IActionResult> LoginCookie([FromBody] UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
            if (user == null)
            {
                _logger.LogWarning($"User not found", userForLoginDto.UserName);
                return Unauthorized(userForLoginDto);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Wrong Password", userForLoginDto.UserName);
                return Unauthorized(userForLoginDto);
            }

            _logger.LogInformation($"USer logged in", userForLoginDto.UserName);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = userForLoginDto.RememberMe,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });;
            return Accepted();

        }

        [HttpOptions("logout-cookie")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<IActionResult> LogoutCookie()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Accepted();
        }

        [HttpOptions("access-denied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }

    }
}
