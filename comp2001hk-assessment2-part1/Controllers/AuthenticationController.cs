using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using comp2001hk_assessment2_part1.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace comp2001hk_assessment2_part1.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login", Name = "Login")]
        [SwaggerOperation(Summary = "User login")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login Successful", typeof(Response))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Login Failed", typeof(Response))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager
                .CheckPasswordAsync(user, model.Password))
            {
                var role = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                foreach (var r in role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, r));
                }

                var token = GetToken(claims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new Response
            { Status = "Error", Message = "Authentication failure. Missing or invalid token" });
        }

        [HttpPost("register", Name = "Register")]
        [SwaggerOperation(Summary = "New user register")]
        [SwaggerResponse(StatusCodes.Status200OK, "User created successfully", typeof(Response))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "User created failed", typeof(Response))]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var validUser = await _userManager.FindByNameAsync(model.Username);

            if (validUser != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = "User already exist." });
            }

            IdentityUser user = new()
            {
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User create failed. Please check password strength. " +
                "A strong password must be at least 8 characters long including uppercase letters, " +
                "lowercase letters, numbers and characters."
                });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost("admin-register", Name = "AdminRegister")]
        [SwaggerOperation(Summary = "New admin register")]
        [SwaggerResponse(StatusCodes.Status200OK, "User created successfully", typeof(Response))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "User created failed", typeof(Response))]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterModel model)
        {
            var validUser = await _userManager.FindByNameAsync(model.Username);

            if (validUser != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = "User already exist." });
            }

            IdentityUser user = new()
            {
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User create failed. Please check password strength. " +
                "A strong password must be at least 8 characters long including uppercase letters, " +
                "lowercase letters, numbers and characters."
                });
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Student))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Student);
            }

            return Ok(new Response { Status = "Success", Message = "Admin user created successfully" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;

        }
    }
}
