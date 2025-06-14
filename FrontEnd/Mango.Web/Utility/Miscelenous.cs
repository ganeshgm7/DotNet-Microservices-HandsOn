namespace Mango.Web.Utility;

public class Miscelenous
{
    public static string CouponAPIBaseUrl { get; set; }
    public static string AuthAPIBaseUrl { get; set; }

    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string TokenCookie = "JwtToken";

    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
