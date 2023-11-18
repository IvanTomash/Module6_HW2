namespace MVC.Models.Responses
{
    public class ItemsResponse<T>
    {
        public long Count { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
