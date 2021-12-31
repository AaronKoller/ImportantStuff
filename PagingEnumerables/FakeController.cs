using System;
using System.Collections.Generic;

namespace PagingEnumerables
{
    public class FakeController
    {
        private readonly IEnumerablePager<int> _enumerablePager;

        public FakeController() : this(new EnumerablePager<int>(new CacheProvider<DataPage<int>>())) { }

        public FakeController(IEnumerablePager<int> enumerablePager)
        {
            _enumerablePager = enumerablePager;
        }

        public ResponseObject<int> GetResponse()
        {
            IEnumerable<int> data = Array.Empty<int>();
            var firstPageOfData = _enumerablePager.GetFirstPageAndContinueInBackground(data);
            var responseObject = new ResponseObject<int>
            {
                NextUrl = $"/api/Thing/{firstPageOfData.NextPageId}",
                Data = firstPageOfData.PageOfData,
                MaxPageSize = _enumerablePager.MaxPageSize,
            };

            //2nd Query to Database to get the total number of Records for this request
            responseObject.NumOfTotalRecordS = 100;
            return responseObject;
        }
    }
}