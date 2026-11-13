   namespace Pschool.Shared.Dtos;

   // One page of results, plus what the frontend needs to show paging controls.
   // T is the kind of row: ParentResponse or StudentResponse.
   public class PagedResult<T>
   {
       // The rows on this page.
       public List<T> Items { get; set; } = new();

       // Which page this is (starting at 1) and how many rows per page.
       public int Page { get; set; }
       public int PageSize { get; set; }

       // How many rows exist in total, across all pages.
       public int TotalCount { get; set; }

       // Calculated from the values above, e.g. 23 rows at 10 per page = 3 pages.
       public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
   }