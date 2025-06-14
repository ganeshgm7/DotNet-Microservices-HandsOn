using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.Miscelenous;

namespace Mango.Web.Service;

public class AuthService(IBaseService baseService) : IAuthService
{
    private readonly IBaseService _baseService = baseService;

    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registerRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = registerRequestDto,
            Url = AuthAPIBaseUrl + "/api/auth/assignrole"
        });
    }

    public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = loginRequestDto,
            Url = AuthAPIBaseUrl + "/api/auth/login"
        });
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = registerRequestDto,
            Url = AuthAPIBaseUrl + "/api/auth/register"
        });
    }
}
