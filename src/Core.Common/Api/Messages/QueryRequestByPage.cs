using System.ComponentModel.DataAnnotations;

namespace Core.Common
{
    public class QueryRequestByPage
    {
        [Range(1, int.MaxValue)]
        public virtual int PageIndex { get; set; } = 1;
        [Range(1, 100)]
        public virtual int PageSize { get; set; } = 10;
        public virtual string Keyword { get; set; }
    }
}
