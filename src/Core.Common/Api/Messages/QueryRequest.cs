using System.ComponentModel.DataAnnotations;

namespace Core.Common.Messages
{
    public class QueryRequest
    {
        [Range(1, 100)]
        public int Taken { get; set; }
    }
}



