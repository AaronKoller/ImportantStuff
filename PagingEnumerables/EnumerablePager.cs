using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagingEnumerables
{
    public interface IEnumerablePager<T>
    {
        public int MaxPageSize { get; }
        DataPage<T> GetFirstPageAndContinueInBackground(IEnumerable<T> data);
    }

    public class EnumerablePager<T> : IEnumerablePager<T>
    {
        public int MaxPageSize { get; } = 10;

        private readonly ICacheProvider<DataPage<T>> _cacheProvider;

        public EnumerablePager(ICacheProvider<DataPage<T>> cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public DataPage<T> GetFirstPageAndContinueInBackground(IEnumerable<T> data)
        {
            var enumerator = data.GetEnumerator();
            var isMoreStuff = enumerator.MoveNext();

            IEnumerable<T> GetNextPageOfData()
            {
                var count = 0;
                while (isMoreStuff && count < MaxPageSize)
                {
                    yield return enumerator.Current;
                    count++;
                    isMoreStuff = enumerator.MoveNext();
                }
            }

            var firstPageOfData = GetNextPageOfData().ToArray();
            if (!isMoreStuff)
            {
                var dataPage = new DataPage<T>(firstPageOfData, Guid.Empty);
                return dataPage;
            }

            var nextPageId = Guid.NewGuid();
            var firstDataPage = new DataPage<T>(firstPageOfData, nextPageId);

            new TaskFactory().StartNew(() =>
            {
                while (isMoreStuff)
                {
                    var currentPageId = nextPageId;

                    var pageOfData = GetNextPageOfData().ToArray();
                    nextPageId = isMoreStuff ? Guid.NewGuid() : Guid.Empty;
                    var dataPage = new DataPage<T>(pageOfData, nextPageId);

                    _cacheProvider.Save(currentPageId.ToString(), dataPage);
                }
            });

            return firstDataPage;
        }
    }
}
