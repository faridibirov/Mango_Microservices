using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDTO,
            Url = SD.AuthAPIBase + "/api/auth/assignRole"
        });
    }

    public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = loginRequestDto,
            Url = SD.AuthAPIBase + "/api/auth/login"
        });
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDTO,
            Url = SD.AuthAPIBase + "/api/auth/register"
        });
    }
}
