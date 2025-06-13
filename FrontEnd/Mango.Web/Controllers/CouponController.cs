using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Mango.Web.Controllers;
public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto>? couponList = [];
        ResponseDto? response = await _couponService.GetAllCoupons();

        if (response is not null && response.IsSuccess)
        {
            couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(couponList);
    }

    public async Task<IActionResult> CreateCoupon()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
    {
        if (ModelState.IsValid)
        {
            List<CouponDto>? couponList = [];
            ResponseDto? response = await _couponService.CreateCoupon(couponDto);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Created Successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

        }
        return View();
    }

    public async Task<IActionResult> DeleteCoupon(int couponId)
    {
        ResponseDto? response = await _couponService.GetCouponById(couponId);

        if (response is not null && response.IsSuccess)
        {
            CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            return View(couponDto);
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCoupon(CouponDto couponDto)
    {
        ResponseDto? response = await _couponService.DeleteCoupon(couponDto.CouponId);

        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "Coupon Deleted Successfully";

            return RedirectToAction(nameof(CouponIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(couponDto);
    }
}
