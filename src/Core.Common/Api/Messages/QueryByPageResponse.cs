namespace Core.Common.Messages
{
    public class QueryByPageResponse<TItem> : QueryResponse<TItem>
    {
        public int Total { get; set; }
    }
}



