namespace Core.Entities
{
    public class PaginatedResult<T>
    {
        public int TotalCount{get;set;}
        public int PageSize{get;set;}
        public int PageIndex{get;set;}
        public IReadOnlyList<T> Items{get;set;} = [];
        
    }
}