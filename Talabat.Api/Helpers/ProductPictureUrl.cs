﻿using AutoMapper;
using AutoMapper.Execution;
using Talabat.Api.Dtos.ProductDto;
using Talabat.Core.Entities;

namespace Talabat.Api.Helpers
{
    public class ProductPictureUrl : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;
        public ProductPictureUrl( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl)) {
                return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty ;

        }
    }
}