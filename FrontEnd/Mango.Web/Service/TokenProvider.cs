﻿using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Newtonsoft.Json.Linq;

namespace Mango.Web.Service;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void ClearToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(Miscelenous.TokenCookie);
    }

    public string? GetToken()
    {
        string? token = null;

        bool? hastoken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Miscelenous.TokenCookie, out token);

        return hastoken is true ? token : null;
    }

    public void SetToken(string token)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(Miscelenous.TokenCookie, token);
    }
}
