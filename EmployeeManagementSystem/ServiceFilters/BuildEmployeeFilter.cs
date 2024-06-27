using EmployeeManagementSystem.Common;
using EmployeeManagementSystem.Entities;
using System.Linq;

namespace EmployeeManagementSystem.ServiceFilters
{
    public static class BuildEmployeeFilter
    {
        public static string Build(FilterCriteria criteria)
        {
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(criteria.FilterAttribute))
            {
                filters.Add($"c.{criteria.FilterAttribute} != null");
            }

            return string.Join(" AND ", filters);
        }
    }
}
