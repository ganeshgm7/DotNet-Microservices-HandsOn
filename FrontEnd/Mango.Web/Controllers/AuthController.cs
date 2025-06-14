using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Web.Controllers;
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }


    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

        if (responseDto != null && responseDto.IsSuccess)
        {
            LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

            await SignInUser(loginResponseDto);
            _tokenProvider.SetToken(loginResponseDto.Token);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("CustomError", responseDto?.Message ?? "Login failed. Please try again.");
            return View(loginRequestDto);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        List<SelectListItem> roleList = new()
        {
            new SelectListItem() { Text =Miscelenous.RoleAdmin, Value = Miscelenous.RoleAdmin },
            new SelectListItem() { Text = Miscelenous.RoleCustomer, Value = Miscelenous.RoleCustomer }
        };

        ViewBag.RoleList = roleList;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
    {
        ResponseDto responseDto = await _authService.RegisterAsync(registrationRequestDto);
        ResponseDto assignRole;

        if (responseDto is not null && responseDto.IsSuccess)
        {
            if (string.IsNullOrEmpty(registrationRequestDto.Role))
            {
                registrationRequestDto.Role = Miscelenous.RoleCustomer;
            }

            assignRole = await _authService.AssignRoleAsync(registrationRequestDto);

            if (assignRole != null && assignRole.IsSuccess)
            {
                TempData["success"] = "Registration successful";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["error"] = assignRole?.Message ?? "Role assignment failed.";
            }

            List<SelectListItem> roleList =
                    [
                        new SelectListItem() { Text =Miscelenous.RoleAdmin, Value = Miscelenous.RoleAdmin },
                        new SelectListItem() { Text = Miscelenous.RoleCustomer, Value = Miscelenous.RoleCustomer }
                    ];

            ViewBag.RoleList = roleList;
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInUser(LoginResponseDto loginResponseDto)
    {
        JwtSecurityTokenHandler handler = new();

        JwtSecurityToken jwt = handler.ReadJwtToken(loginResponseDto.Token);

        ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
            jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
            jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
            jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
          jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

        ClaimsPrincipal principal = new(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
