﻿using AutoMapper;
using Mango.Service.CouponAPI.Context;
using Mango.Service.CouponAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.CouponAPI.Respository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationContext _db;
        private readonly IMapper _mapper;
        public CouponRepository(ApplicationContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<CouponDto> GetCouponByCodeAsync(string couponCode)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(c=>c.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
