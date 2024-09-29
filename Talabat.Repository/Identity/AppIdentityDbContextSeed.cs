using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
 public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserasync(UserManager<AppUser> userManager)
        {
           
            if(userManager.Users.Count()==0)
            {
                var user = new AppUser()
                {
                    DisplayName = "ahmedmohamed",
                    Email = "ahmed33@gmail.com",
                    UserName = "ahmedmohamed",
                    PhoneNumber = "010022556677"
                };
           var res=await userManager.CreateAsync(user,"Pa$w0rd");
               
            }
           
        }
    }
}
