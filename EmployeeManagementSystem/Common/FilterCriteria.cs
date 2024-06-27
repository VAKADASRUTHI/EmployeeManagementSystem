namespace EmployeeManagementSystem.ServiceFilters
{
    public class FilterCriteria
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public string FilterAttribute { get; set; }
    }
}