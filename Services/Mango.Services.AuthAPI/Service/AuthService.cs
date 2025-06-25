using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext appDbContext, IJwtTokenGenerator jwtTokenGenerator,
                       UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        ApplicationUser? user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user is not null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        return false;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        ApplicationUser? user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || !isValid)
        {
            return new LoginResponseDto()
            {
                User = null,
                Token = string.Empty
            };
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string token = _jwtTokenGenerator.GenerateToken(user, roles);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };

        return new LoginResponseDto()
        {
            User = userDto,
            Token = token // Token generation logic should be implemented here
        };
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser applicationUser = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.PhoneNumber
        };

        try
        {
            IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, registrationRequestDto.Password);

            if (identityResult.Succeeded)
            {
                ApplicationUser? user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                };

                return "";
            }
            else
            {
                return identityResult.Errors.FirstOrDefault()?.Description;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
