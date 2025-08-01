namespace Dsw2025Tpi.Application.Dtos
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Purchases { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PagedResult(IEnumerable<T> purchases, int totalCount, int page, int pageSize)
        {
            Purchases = purchases;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}