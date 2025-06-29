﻿using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mapConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Coupon, CouponDto>();
            config.CreateMap<CouponDto, Coupon>();
        });

        return mapConfig;
    }
}
