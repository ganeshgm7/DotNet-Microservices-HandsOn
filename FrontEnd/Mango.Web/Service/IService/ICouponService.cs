using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface ICouponService
{
    Task<ResponseDto?> GetCoupon(string couponCode);
    Task<ResponseDto?> GetAllCoupons();
    Task<ResponseDto?> GetCouponById(int couponId);
    Task<ResponseDto?> CreateCoupon(CouponDto couponDto);
    Task<ResponseDto?> UpdateCoupon(CouponDto couponDto);
    Task<ResponseDto?> DeleteCoupon(int couponId);
}
