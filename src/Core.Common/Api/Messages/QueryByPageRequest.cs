using System.ComponentModel.DataAnnotations;

namespace Core.Common.Messages
{
    public class QueryByPageRequest
    {
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
        public int Offset { get { return (PageIndex - 1) * PageSize; } }
    }
}
