namespace Catalog.Host.Models.Response
{
    public class ItemsResponse<T>
    {
        public long Count { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
