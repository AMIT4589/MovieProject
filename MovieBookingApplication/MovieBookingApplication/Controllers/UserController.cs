using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieBookingApplication.BookingModels;
using MovieBookingApplication.BookingModels.DataTransferObjects;
using MovieBookingApplication.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBookingApplication.Controllers
{


    [Route("api/v1.0/moviebooking")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISetupJWT _jwtConfig;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<UserRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ISetupJWT jwtConfig)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(SignInDataTransferObject model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Username or Password is not valid!");
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Invalid login attempt!");
            }
            var success = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!success)
            {
                return BadRequest("Invalid login attempt!");
            }
            var token = GenerateToken(user);
            return Ok(new { accessToken = token });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(SignUpDataTransferObject model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("User Credentials not valid!");
            }
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("User already exists!");
            }
            var role = await _roleManager.FindByNameAsync("User");
            if (role == null) await _roleManager.CreateAsync(new UserRole() { Name = "User" });
            await _userManager.AddToRoleAsync(user, "User");
            var token = GenerateToken(user);
            return Ok(new { accessToken = token });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotDataTransferObject model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email is not a valid email!");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // send mail to register with this email
                return NotFound();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new { passwordResetCode = code });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data is not valid!");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid Credentials!");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid Credentials!");
            }
            return Ok("Your password reset successfully. Try to Login");
        }

        #region Helpers

        private string GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };
            foreach (var roleId in user.Roles)
            {
                var role = _roleManager.FindByIdAsync(roleId).Result;
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            var secretBytes = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signInCredentials = new SigningCredentials(key, algorithm);
            var token = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signInCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}


