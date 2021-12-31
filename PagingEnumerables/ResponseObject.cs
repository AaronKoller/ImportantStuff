using System.Collections.Generic;

namespace PagingEnumerables
{
    public class ResponseObject<T>
    {
        public string NextUrl { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int MaxPageSize { get; set; }
        public int NumOfTotalRecordS { get; set; }
    }
}