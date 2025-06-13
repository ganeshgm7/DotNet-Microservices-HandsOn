using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.Miscelenous;

namespace Mango.Web.Service;

public class CouponService(IBaseService baseService) : ICouponService
{
    private readonly IBaseService _baseService = baseService;

    public Task<ResponseDto?> CreateCoupon(CouponDto couponDto)
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = couponDto,
            Url = CouponAPIBaseUrl + "/api/coupon"
        });
    }

    public Task<ResponseDto?> DeleteCoupon(int couponId)
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.DELETE,
            Url = CouponAPIBaseUrl + $"/api/coupon/{couponId}"
        });
    }

    public Task<ResponseDto?> GetAllCoupons()
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBaseUrl + "/api/coupon"
        });
    }

    public Task<ResponseDto?> GetCoupon(string couponCode)
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBaseUrl + $"/api/coupon/code/{couponCode}"
        });
    }

    public Task<ResponseDto?> GetCouponById(int couponId)
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBaseUrl + $"/api/coupon/{couponId}"
        });
    }

    public Task<ResponseDto?> UpdateCoupon(CouponDto couponDto)
    {
        return _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Data = couponDto,
            Url = CouponAPIBaseUrl + "/api/coupon"
        });
    }
}
