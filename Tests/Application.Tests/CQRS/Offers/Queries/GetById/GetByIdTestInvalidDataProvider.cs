using System;
using System.Collections;
using Application.CQRS.Offers.Queries.GetById;

namespace Application.Tests.CQRS.Offers.Queries.GetById
{
    public class GetByIdTestInvalidDataProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new[]
            {
                new GetByIdQuery
                {

                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

