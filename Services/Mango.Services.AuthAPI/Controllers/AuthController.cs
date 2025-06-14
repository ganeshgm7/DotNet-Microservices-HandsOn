using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ResponseDto _responseDto;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
        _responseDto = new ResponseDto();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        string resultMessage = await _authService.Register(registrationRequestDto);

        if (!string.IsNullOrEmpty(resultMessage))
        {
            _responseDto.IsSuccess = false;
            _responseDto.Result = resultMessage;
            return Ok(_responseDto);
        }

        return Ok(_responseDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        LoginResponseDto loginResponseDto = await _authService.Login(loginRequestDto);

        if (loginResponseDto.User == null)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Result = "Username or password is incorrect";
            return BadRequest(_responseDto);
        }

        _responseDto.Result = loginResponseDto;
        _responseDto.IsSuccess = true;

        return Ok(_responseDto);
    }

    [HttpPost("assignrole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        bool assignRoleResponse = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());

        if (!assignRoleResponse)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Result = "Role assignment failed";
            return BadRequest(_responseDto);
        }

        return Ok(_responseDto);
    }
}
