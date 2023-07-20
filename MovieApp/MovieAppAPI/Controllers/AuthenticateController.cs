using Common.Enums;
using Common.ViewModels;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName??throw new NullReferenceException()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserId", user.Id) // Add UserId as a claim
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"] ?? throw new NullReferenceException()));
                var token = new JwtSecurityToken
                    (
                        issuer: _configuration["JWTSettings:Issuer"],
                        audience: _configuration["JWTSettings:Audience"],
                        expires: DateTime.Now.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        
        
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<string> { Status = "Error", Message = "User Already Exists" });
            }
            Users user = new ()
            {
                Email = register.Email,
                UserName = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<string> { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            result = await _userManager.AddToRoleAsync(user, UserRole.Basic_User.ToString());
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<string> { Status = "Error", Message = "Cannot assign role to the user" });
            }

            return Ok(new Response<string> { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("~/api/Authentication/Logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and invalidate the authentication cookie
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

            return Ok(new { Message = "Logout successful" });
        }
    }
}
