using System;
using System.Collections.Generic;

namespace Core
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList()
            : this(new List<T>(), 1, 1, 0)
        {

        }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            AddRange(source);
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPageCount; }
        }

        public int RowNumber(T item)
        {
            return IndexOf(item) + 1 + (PageIndex - 1) * PageSize;
        }
    }
}