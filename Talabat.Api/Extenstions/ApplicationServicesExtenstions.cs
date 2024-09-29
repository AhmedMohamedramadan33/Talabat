using Microsoft.AspNetCore.Mvc;
using Talabat.Api.AutoMapper;
using Talabat.Api.Errors;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.GenericRepository.ServicesContract;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Data.UnitOfWork;
using Talabat.Service;

namespace Talabat.Api.Extentions
{
    public static class ApplicationServicesExtenstions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IpaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWorkIm));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));

            //services.AddScoped(typeof(IgenericRepositories<>), typeof(GenericRepositories<>));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(op =>
            {
                op.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(x => x.Value.Errors.Count() > 0).
                    SelectMany(x => x.Value.Errors).
                    Select(x => x.ErrorMessage).
                    ToArray();
                    var obj = new ApiValidationErrorResponse() { Errors = errors };
                    return new BadRequestObjectResult(obj);
                };
            });
            return services;
        }
    }
}
