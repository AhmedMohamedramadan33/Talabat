using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Data.Repositories
{
   public interface ICacheResponseService
    {
        Task CacheResponse(string cachekey, object response, TimeSpan timetolive);
        Task<string?> Getcachedresponse(string cachekey);
    }
}
