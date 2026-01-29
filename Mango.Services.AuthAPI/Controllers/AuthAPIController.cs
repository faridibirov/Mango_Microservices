using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Mango.Services.AuthAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly IAuthService _authService;
    protected ResponseDto _responseDto;
    private IConfiguration _configuration;
    private readonly IMessageBus _messageBus;

    public AuthAPIController(IAuthService authService, IConfiguration configuration, IMessageBus messageBus)
    {
        _authService = authService;
        _responseDto = new();
        _configuration = configuration;
        _messageBus = messageBus;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
    {

        var errorMessage = await _authService.Register(model);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            _responseDto.IsSuccess =  false;
            _responseDto.Message = errorMessage;
            return BadRequest(_responseDto);
        }
        await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueuNames:RegisterUserQueue"));
        return Ok(_responseDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var loginResponse = await _authService.Login(model);

        if (loginResponse.User==null)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Username or password is incorrect";
            return BadRequest(_responseDto);
        }
        _responseDto.Result = loginResponse;
        return Ok(_responseDto);
    }


    [HttpPost("assignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
    {

        var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

        if (!assignRoleSuccessful)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error encountered";
            return BadRequest(_responseDto);
        }

        return Ok(_responseDto);
    }
}
