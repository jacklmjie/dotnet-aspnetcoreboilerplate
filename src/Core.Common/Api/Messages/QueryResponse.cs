using System.Collections.Generic;

namespace Core.Common.Messages
{
    public class QueryResponse<TItem>
    {
        public IEnumerable<TItem> List { get; set; }
    }
}



