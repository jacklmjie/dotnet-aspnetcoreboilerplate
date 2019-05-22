using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{
    public class QueryResponseByPage<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public QueryResponseByPage(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static QueryResponseByPage<T> Create(
            int count, IEnumerable<T> items, QueryRequestByPage reqMsg)
        {
            return new QueryResponseByPage<T>(items.ToList(), count, reqMsg.PageIndex, reqMsg.PageSize);
        }
    }
}



