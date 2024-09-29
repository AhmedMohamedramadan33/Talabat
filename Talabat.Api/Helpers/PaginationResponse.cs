using Talabat.Api.Dtos.ProductDto;

namespace Talabat.Api.Helpers
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count= count;
            Data = data;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; }

        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
