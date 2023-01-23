using AutoMapper;
using FootballScoresDbApi.Models;
using FootballScoresDbApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FootballScoresDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt has been received for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = _mapper.Map<ApplicationUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Register)}");
                return Problem($"Error in {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginDTO)
        {
            _logger.LogInformation($"Login attempt has been received for {loginDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

                if (!result.Succeeded)
                {
                    return Unauthorized(loginDTO);
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Login)}");
            }
            return Problem($"Error in {nameof(Login)}", statusCode: 500);
        }
    }
}
