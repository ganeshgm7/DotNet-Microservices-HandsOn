using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Model;
using Mango.Services.CouponAPI.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly ResponseDto _responseDto;
    private IMapper _mapper;

    public CouponController(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _responseDto = new ResponseDto();
        _mapper = mapper;
    }

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            IEnumerable<Coupon> coupons = _appDbContext.Coupons.ToList();

            _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpGet]
    [Route("{couponId:int}")]
    public ResponseDto Get(int couponId)
    {
        try
        {
            Coupon coupon = _appDbContext.Coupons.First(x => x.CouponId == couponId);
            _responseDto.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpGet]
    [Route("code/{code}")]
    public ResponseDto Get(string code)
    {
        try
        {
            Coupon coupon = _appDbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
            _responseDto.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpPost]
    public ResponseDto Post([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);

            _appDbContext.Coupons.Add(coupon);
            _appDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpPut]
    public ResponseDto Put([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);

            _appDbContext.Coupons.Update(coupon);
            _appDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpDelete]
    [Route("{couponId:int}")]
    public ResponseDto Delete(int couponId)
    {
        try
        {
            Coupon coupon = _appDbContext.Coupons.First(x => x.CouponId == couponId);

            _appDbContext.Coupons.Remove(coupon);
            _appDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }
}
