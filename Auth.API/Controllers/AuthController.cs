using Auth.API.Configurations;
using Auth.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new RegisterResponseDto { Result = false, Errors = ModelState.Select(x => x.ToString()).ToList() });

            var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);

            if (emailExist is not null)
                return BadRequest(new RegisterResponseDto { Result = false, Errors = new List<string> { "Email already exist" } });

            var newUser = new IdentityUser()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };

            var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);

            if (!isCreated.Succeeded)
                return BadRequest(new RegisterResponseDto { Result = false, Errors = isCreated.Errors.Select(x => x.Description.ToString()).ToList() });

            return Ok(new RegisterResponseDto()
            {
                Result = true,
                Token = GenerateJwtToken(newUser)
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginResponseDto { Result = false, Errors = ModelState.Select(x => x.ToString()).ToList() });

            var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);

            if (emailExist is null)
                return BadRequest(new LoginResponseDto { Result = false, Errors = new List<string> { "Invalid authentication" } });

            var isPasswordValid = await _userManager.CheckPasswordAsync(emailExist, requestDto.Password);

            if (!isPasswordValid)
                return BadRequest(new LoginResponseDto { Result = false, Errors = new List<string> { "Invalid authentication" } });

            return Ok(new LoginResponseDto{
                Result = true,
                Token = GenerateJwtToken(emailExist)
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
