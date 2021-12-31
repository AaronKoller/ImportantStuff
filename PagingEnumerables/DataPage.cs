using System;
using System.Collections.Generic;

namespace PagingEnumerables
{
    public record DataPage<T>(IEnumerable<T> PageOfData, Guid NextPageId);
}