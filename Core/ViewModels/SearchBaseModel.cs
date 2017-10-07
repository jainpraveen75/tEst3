using Core.Enums;

namespace Core.ViewModels
{
    public abstract class SearchBaseModel
    {
        private const int Pagesize = 10;

        protected SearchBaseModel()
            : this(1, "", OrderBy.Ascending)
        {
        }

        protected SearchBaseModel(int page, string sortColumn, OrderBy direction)
        {
            Page = page;
            SortColumn = sortColumn;
            Direction = direction;
            PageSize = Pagesize;
        }

        public int Page { get; set; }
        public string SortColumn { get; set; }
        public OrderBy Direction { get; set; }
        public int PageSize { get; set; }
    }
}
