using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Data.Specification
{
    public class ProductSpecParam
    {
        const int maxsize = 5;
        int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? maxsize : value; }
        }

        string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public string? Name { get; set; }
        public int PageIndex { get; set; } = 1;
        public string? sort { get; set; }
        public int? Brandid { get; set; }
        public int? CategoryId { get; set; }
    }
}
