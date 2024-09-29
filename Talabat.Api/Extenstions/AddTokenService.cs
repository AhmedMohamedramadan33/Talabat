using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Talabat.Api.Extenstions
{
    public static class AddTokenService
    {

        public static IServiceCollection AddTokenServicesExtenstions(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
       .AddJwtBearer(opt =>
       {
           opt.TokenValidationParameters = new TokenValidationParameters()
           {
               ValidateAudience = true,
               ValidAudience = configuration["JWT:Audience"],
               ValidateIssuer = true,
               ValidIssuer = configuration["JWT:Issuer"],
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
               ValidateLifetime = true,
               ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:Expire"]))
           };
       });
            return builder;
        }
    }
}
