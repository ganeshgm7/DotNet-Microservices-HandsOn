﻿using static Mango.Web.Utility.Miscelenous;

namespace Mango.Web.Models;

public class RequestDto
{
    public ApiType ApiType { get; set; } = ApiType.GET;

    public string Url { get; set; }

    public object Data { get; set; }

    public string AccessToken { get; set; }

}
