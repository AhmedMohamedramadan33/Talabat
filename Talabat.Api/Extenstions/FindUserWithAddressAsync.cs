using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Api.Dtos.OrderDtos;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Api.Extenstions
{
    public static class FindUserWithAddressAsync
    {
        public static async Task<AppUser?> FindUserByEmailAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal user)
        {
            var email=user.FindFirstValue(ClaimTypes.Email);
            var ResUser=await userManager.Users.Include(x=>x.Address).Where(x=>x.Email==email).SingleOrDefaultAsync();
            return ResUser;

        }
    }
}
