public class TaskQueryParams
{
    public int ProjectId { get; set; }  
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Status { get; set; }

    public string? SortBy { get; set; } = "createdAt";
    public bool Desc { get; set; } = true;
}